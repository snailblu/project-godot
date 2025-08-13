using Godot;
using System;
using projectgodot.Components;
using projectgodot.Constants;
using projectgodot.Utils;
using projectgodot.Scripts.Interfaces;

namespace projectgodot
{
    public partial class Player : CharacterBody2D
    {
        // 인터페이스 기반 컴포넌트들
        private IMovement _playerMovement;
        private IWeapon _weapon;
        private IHealth _health;
        private IPlayerInput _inputHandler;
        private IPlayerAnimation _animationController;
        private IStateMachine _stateMachine;
        
        // Export NodePath들 (의존성 주입용)
        [Export] public NodePath MovementComponentPath;
        [Export] public NodePath WeaponComponentPath;
        [Export] public NodePath HealthComponentPath;
        [Export] public NodePath InputHandlerPath;
        [Export] public NodePath AnimationControllerPath;
        [Export] public NodePath StateMachinePath;
        
        // 기타 컴포넌트들 (아직 인터페이스화되지 않은 것들)
        private ISceneFactory _sceneFactory;
        private CameraShakeComponent _cameraShake;
        private PlayerCollisionHandler _collisionHandler;
        private PlayerEventBridge _eventBridge;
        
        
        // 호환성을 위한 프로퍼티들
        public IHealth Health => _health;

        private AnimationTree _animationTree;

        public override void _Ready()
        {
            InitializeComponents();
            InitializeStateMachine();
            InitializeEventHandlers();
        }

        /// <summary>
        /// NodePath로 컴포넌트를 해석하거나 fallback 인스턴스를 생성하는 헬퍼 메서드
        /// </summary>
        private T ResolveOrFallback<T>(NodePath path, Func<T> fallback) where T : class
        {
            if (path != null && !path.IsEmpty)
            {
                try
                {
                    var node = GetNodeOrNull(path);
                    if (node is T component) 
                        return component;
                }
                catch
                {
                    // 안전하게 무시하고 fallback으로 진행
                }
            }
            return fallback();
        }

        private void InitializeComponents()
        {
            
            // 인터페이스 기반 컴포넌트들을 NodePath로 주입 (ResolveOrFallback 헬퍼 사용)
            _playerMovement = ResolveOrFallback(MovementComponentPath, () => new PlayerMovement());
            _weapon = ResolveOrFallback(WeaponComponentPath, () => new WeaponComponent(cooldown: 0.25f));
            _health = ResolveOrFallback(HealthComponentPath, () => new HealthComponent(100));
            
            // UI 및 효과 컴포넌트들 (아직 인터페이스화되지 않음)
            if (InputHandlerPath != null && !InputHandlerPath.IsEmpty)
            {
                var inputNode = GetNode(InputHandlerPath);
                _inputHandler = inputNode as IPlayerInput;
            }
            if (_inputHandler == null)
            {
                var inputHandler = new PlayerInputHandler();
                AddChild(inputHandler);
                _inputHandler = inputHandler;
            }
            
            if (AnimationControllerPath != null && !AnimationControllerPath.IsEmpty)
            {
                var animNode = GetNode(AnimationControllerPath);
                _animationController = animNode as IPlayerAnimation;
            }
            if (_animationController == null)
            {
                var animController = new PlayerAnimationController();
                AddChild(animController);
                _animationController = animController;
            }
            
            _collisionHandler = new PlayerCollisionHandler();
            _eventBridge = new PlayerEventBridge();
            _cameraShake = new CameraShakeComponent();
            
            // SceneFactory 찾기 (기존 방식 유지)
            var sceneRoot = GetTree().CurrentScene;
            _sceneFactory = sceneRoot.FindChild("SceneFactory", true) as ISceneFactory;
            if (_sceneFactory == null)
            {
                GodotLogger.SafePrint("SceneFactory를 찾을 수 없습니다.");
            }
            
            // IGameComponent를 구현한 컴포넌트들 초기화
            InitializeGameComponents();
        }
        
        /// <summary>
        /// IGameComponent를 구현한 컴포넌트들을 초기화
        /// </summary>
        private void InitializeGameComponents()
        {
            // 각 컴포넌트가 IGameComponent를 구현하는 경우 초기화
            if (_playerMovement is IGameComponent movementComponent)
                movementComponent.Initialize(this);
                
            if (_weapon is IGameComponent weaponComponent)
                weaponComponent.Initialize(this);
                
            if (_health is IGameComponent healthComponent)
                healthComponent.Initialize(this);
                
                
            if (_inputHandler is IGameComponent inputComponent)
                inputComponent.Initialize(this);
                
            if (_animationController is IGameComponent animationComponent)
                animationComponent.Initialize(this);
        }

        private void InitializeStateMachine()
        {
            // StateMachine을 먼저 추가하고 초기화
            PlayerStateMachine stateMachineNode;
            if (StateMachinePath != null && !StateMachinePath.IsEmpty)
            {
                var smNode = GetNode(StateMachinePath);
                stateMachineNode = smNode as PlayerStateMachine;
                if (stateMachineNode == null)
                {
                    stateMachineNode = new PlayerStateMachine();
                    AddChild(stateMachineNode);
                }
            }
            else
            {
                stateMachineNode = new PlayerStateMachine();
                AddChild(stateMachineNode);
            }
            _stateMachine = stateMachineNode;
            
            // 다른 컴포넌트들 추가 (Node 타입만)
            AddChild(_cameraShake);
            if (_inputHandler is Node inputNode && inputNode.GetParent() == null)
                AddChild(inputNode);
            if (_animationController is Node animNode && animNode.GetParent() == null)
                AddChild(animNode);
            AddChild(_collisionHandler);
            AddChild(_eventBridge);
            
            // 애니메이션 시스템 초기화
            _animationTree = GetNode<AnimationTree>("AnimationTree");
            _animationController.Initialize(_animationTree);
            
            // StateMachine 연결
            _collisionHandler.Initialize(stateMachineNode);
            _inputHandler.Initialize(stateMachineNode);
        }

        private void InitializeEventHandlers()
        {
            // 컴포넌트 이벤트 연결
            _weapon.OnShoot += SpawnProjectile;
            Health.Died += OnDeath;
            Health.HealthChanged += OnHealthChanged;
            
            // 입력 이벤트 연결
            _inputHandler.MovementRequested += OnMovementRequested;
            _inputHandler.ShootRequested += OnShootRequested;
            _collisionHandler.DamageReceived += OnDamageReceived;

            // 전역 이벤트 연결
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
            {
                events.CameraShakeRequested += OnCameraShakeRequested;
            }
        }

        private Vector2 _currentMovementDirection;

        public override void _PhysicsProcess(double delta)
        {
            // StateMachine에 현재 입력 상태 업데이트
            _stateMachine?.UpdateMovementInput(_currentMovementDirection.Length() > 0.1f);
            
            // 이동 처리 (StateMachine 상태와 무관하게 물리 처리)
            Vector2 calculatedVelocity;
            
            // StateMachine에서 이동 가능한지 체크
            if (_stateMachine?.CanPerformAction("move") == true)
            {
                {
                    // 일반 이동
                    calculatedVelocity = _playerMovement.CalculateVelocity(
                        _currentMovementDirection, 
                        GameConstants.Player.DEFAULT_MOVEMENT_SPEED, 
                        (float)delta
                    );
                }
            }
            else
            {
                // 이동 불가능한 상태 (Dead, TakingDamage 등)
                calculatedVelocity = Vector2.Zero;
            }

            // Godot 노드에 결과 적용
            Velocity = calculatedVelocity;
            MoveAndSlide();

            // 애니메이션 업데이트
            _animationController.UpdateAnimation(_currentMovementDirection);

            // 무기 쿨다운 업데이트
            _weapon.Process((float)delta);
            
        }

        // 입력 이벤트 핸들러들
        private void OnMovementRequested(Vector2 direction)
        {
            _currentMovementDirection = direction;
        }


        private void OnShootRequested()
        {
            // StateMachine을 통해 발사 상태 전환 요청
            if (_stateMachine?.RequestShoot() == true)
            {
                _weapon.Shoot();
            }
        }

        private void OnDamageReceived(int damage)
        {
            ProcessDamage(damage, "Damage received!");
        }

        private void ProcessDamage(int damage, string logMessage)
        {
            Health.TakeDamage(damage);
            GodotLogger.SafePrint($"{logMessage} Health: {Health.CurrentHealth}/{Health.MaxHealth}");
            // StateMachine을 통해 데미지 상태 전환 요청
            _stateMachine?.RequestTakeDamage();
            _eventBridge.RequestHeavyShake();
        }

        private void OnDeath()
        {
            GodotLogger.SafePrint("Player has died!");
            _eventBridge.NotifyGameOver();
            CallDeferred(MethodName.QueueFree);
        }

        private void SpawnProjectile()
        {
            if (_sceneFactory == null) return;
            
            _eventBridge.NotifyPlayerFiredWeapon();
            _eventBridge.RequestLightShake();
            
            // 총구 화염 효과 생성
            var direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
#if !TEST_ENVIRONMENT
            var muzzleFlashEffect = new MuzzleFlashEffect();
            GetTree().CurrentScene.AddChild(muzzleFlashEffect);
            muzzleFlashEffect.GlobalPosition = GlobalPosition;
            
            // 총구 화염이 총구 방향을 향하도록 회전
            muzzleFlashEffect.Rotation = direction.Angle();
            muzzleFlashEffect.StartEffect();
#endif
            
            // SceneFactory를 통해 발사체 생성 (하드코딩된 리소스 로딩을 ISceneFactory로 위임)
            var projectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/projectile.tscn");
            _sceneFactory.CreateProjectile(projectileScene, GlobalPosition, direction, (int)_weapon.Damage);
        }


        private void OnCameraShakeRequested(float intensity, float duration)
        {
            _cameraShake?.StartShake(intensity, duration);
        }

        private void OnHealthChanged(int currentHealth)
        {
            _eventBridge.NotifyPlayerHealthChanged(currentHealth, Health.MaxHealth);
        }


        public override void _ExitTree()
        {
            // 이벤트 구독 안전하게 해제 (메모리 누수 방지)
            if (_weapon != null) 
                _weapon.OnShoot -= SpawnProjectile;
                
            if (_health != null)
            {
                _health.Died -= OnDeath;
                _health.HealthChanged -= OnHealthChanged;
            }
            
            
            if (_inputHandler != null)
            {
                _inputHandler.MovementRequested -= OnMovementRequested;
                _inputHandler.ShootRequested -= OnShootRequested;
            }
            
            if (_collisionHandler != null)
                _collisionHandler.DamageReceived -= OnDamageReceived;

            // 전역 이벤트 해제
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
                events.CameraShakeRequested -= OnCameraShakeRequested;
                
            base._ExitTree();
        }

    }
}