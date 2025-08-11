using Godot;
using System;
using projectgodot.Components;
using projectgodot.Constants;
using projectgodot.Utils;

namespace projectgodot
{
    public partial class Player : CharacterBody2D
    {
        private PlayerData _playerData;
        private PlayerMovement _playerMovement;
        private WeaponComponent _weapon;
        public DashComponent _dash;
        private PackedScene _projectileScene;
        private ISceneFactory _sceneFactory;
        private PowerupLogic _powerupLogic;
        private float _originalWeaponDamage;
        private CameraShakeComponent _cameraShake;
        
        // 새로운 컴포넌트들
        private PlayerInputHandler _inputHandler;
        private PlayerAnimationController _animationController;
        private PlayerCollisionHandler _collisionHandler;
        private PlayerEventBridge _eventBridge;
        
        // State Machine (중앙화된 상태 관리)
        private PlayerStateMachine _stateMachine;
        
        // 허기 시스템
        public HungerComponent _hungerComponent;
        
        // 누적 굶주림 데미지 (작은 deltaTime 값들을 누적하기 위함)
        private float _accumulatedStarvationDamage = 0f;
        
        public HealthComponent Health { get; private set; }

        private AnimationTree _animationTree;

        public override void _Ready()
        {
            InitializeComponents();
            InitializeStateMachine();
            InitializeEventHandlers();
        }

        private void InitializeComponents()
        {
            // 핵심 컴포넌트 인스턴스화
            _playerData = new PlayerData();
            _playerMovement = new PlayerMovement();
            Health = new HealthComponent(100);
            _dash = new DashComponent();
            _hungerComponent = new HungerComponent(GameConstants.Hunger.DEFAULT_MAX_HUNGER);
            
            // 무기 시스템 초기화
            _weapon = new WeaponComponent(cooldown: 0.25f);
            _originalWeaponDamage = _weapon.Damage;
            _powerupLogic = new PowerupLogic();
            _projectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/projectile.tscn");
            
            // UI 및 효과 컴포넌트
            _inputHandler = new PlayerInputHandler();
            _animationController = new PlayerAnimationController();
            _collisionHandler = new PlayerCollisionHandler();
            _eventBridge = new PlayerEventBridge();
            _cameraShake = new CameraShakeComponent();
            
            // SceneFactory 찾기
            var sceneRoot = GetTree().CurrentScene;
            _sceneFactory = sceneRoot.FindChild("SceneFactory", true) as ISceneFactory;
            if (_sceneFactory == null)
            {
                GodotLogger.SafePrint("SceneFactory를 찾을 수 없습니다.");
            }
        }

        private void InitializeStateMachine()
        {
            // StateMachine을 먼저 추가하고 초기화
            _stateMachine = new PlayerStateMachine();
            AddChild(_stateMachine);
            
            // 다른 컴포넌트들 추가
            AddChild(_dash);
            AddChild(_cameraShake);
            AddChild(_inputHandler);
            AddChild(_animationController);
            AddChild(_collisionHandler);
            AddChild(_eventBridge);
            
            // 애니메이션 시스템 초기화
            _animationTree = GetNode<AnimationTree>("AnimationTree");
            _animationController.Initialize(_animationTree);
            
            // StateMachine 연결
            _collisionHandler.Initialize(_stateMachine);
            _inputHandler.Initialize(_stateMachine);
        }

        private void InitializeEventHandlers()
        {
            // 컴포넌트 이벤트 연결
            _weapon.OnShoot += SpawnProjectile;
            Health.Died += OnDeath;
            Health.HealthChanged += OnHealthChanged;
            _hungerComponent.HungerChanged += OnHungerChanged;
            _hungerComponent.StarvationStarted += OnStarvationStarted;
            
            // 입력 이벤트 연결
            _inputHandler.MovementRequested += OnMovementRequested;
            _inputHandler.DashRequested += OnDashRequested;
            _inputHandler.ShootRequested += OnShootRequested;
            _inputHandler.TestDamageRequested += OnTestDamageRequested;
            _inputHandler.EatFoodRequested += OnEatFoodRequested;
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
                if (_stateMachine.IsInState(PlayerState.Dashing))
                {
                    // 대시 중일 때는 대시 속도 사용
                    calculatedVelocity = _currentMovementDirection * _dash.DashSpeed;
                }
                else
                {
                    // 일반 이동
                    calculatedVelocity = _playerMovement.CalculateVelocity(
                        _currentMovementDirection, 
                        _playerData.MovementSpeed, 
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
            
            // 파워업 효과 업데이트
            UpdatePowerupEffect();
            
            // 허기 시스템 업데이트 (StateMachine과 무관한 백그라운드 프로세스)
            UpdateHungerSystem((float)delta);
            
            // 음식 섭취 입력 처리
            _inputHandler.HandleEatFoodInput();
        }

        // 입력 이벤트 핸들러들
        private void OnMovementRequested(Vector2 direction)
        {
            _currentMovementDirection = direction;
        }

        private void OnDashRequested(Vector2 direction)
        {
            // StateMachine을 통해 대시 상태 전환 요청
            if (_stateMachine?.RequestDash() == true)
            {
                _dash.StartDash(direction);
                _eventBridge.RequestDashShake();
            }
        }

        private void OnShootRequested()
        {
            // StateMachine을 통해 발사 상태 전환 요청
            if (_stateMachine?.RequestShoot() == true)
            {
                _weapon.Shoot();
            }
        }

        private void OnTestDamageRequested()
        {
            ProcessDamage(10, "Player took 10 damage!");
        }

        private void OnEatFoodRequested()
        {
            EatFood();
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
            
            // SceneFactory를 통해 발사체 생성
            _sceneFactory.CreateProjectile(_projectileScene, GlobalPosition, direction, (int)_weapon.Damage);
        }

        private void UpdatePowerupEffect()
        {
            _powerupLogic.Update();
            
            // 파워업 효과가 방금 종료되었는지 확인
            if (!_powerupLogic.IsActive && _weapon.Damage != _originalWeaponDamage)
            {
                // 원래 데미지로 복구
                _weapon.Damage = _originalWeaponDamage;
                GodotLogger.SafePrint($"Powerup effect ended! Damage restored to {_originalWeaponDamage}");
            }
        }
        
        public void ApplyPowerup(float damageMultiplier, float duration)
        {
            if (_powerupLogic.IsActive) 
            {
                GodotLogger.SafePrint("Powerup already active, ignoring new one");
                return;
            }
            
            _powerupLogic.DamageMultiplier = damageMultiplier;
            _powerupLogic.Duration = duration;
            _powerupLogic.Activate(_originalWeaponDamage);
            
            var newDamage = _powerupLogic.CalculateDamage(_originalWeaponDamage);
            _weapon.Damage = newDamage;
            
            GodotLogger.SafePrint($"Powerup applied! Damage: {_originalWeaponDamage} -> {newDamage} for {duration} seconds");
        }

        private void OnCameraShakeRequested(float intensity, float duration)
        {
            _cameraShake?.StartShake(intensity, duration);
        }

        private void OnHealthChanged(int currentHealth)
        {
            _eventBridge.NotifyPlayerHealthChanged(currentHealth, Health.MaxHealth);
        }

        // 허기 시스템 관련 메서드들
        private void UpdateHungerSystem(float deltaTime)
        {
            // 허기 수치 감소는 항상 처리
            _hungerComponent.ProcessHunger(deltaTime, GameConstants.Hunger.HUNGER_DECREASE_RATE);
            
            // StateMachine이 Starving 상태일 때만 굶주림 데미지 적용
            if (_stateMachine?.IsInState(PlayerState.Starving) == true && deltaTime > 0)
            {
                ApplyStarvationDamage(deltaTime);
            }
            else
            {
                // 굶주림 상태가 아닐 때는 누적 데미지 초기화
                _accumulatedStarvationDamage = 0f;
            }
        }

        private void ApplyStarvationDamage(float deltaTime)
        {
            // 누적 방식으로 굶주림 데미지 처리
            _accumulatedStarvationDamage += GameConstants.Hunger.STARVATION_DAMAGE_RATE * deltaTime;
            
            // 누적된 데미지가 1 이상일 때 체력 감소 실행
            if (_accumulatedStarvationDamage >= 1.0f)
            {
                int damageToApply = (int)Math.Floor(_accumulatedStarvationDamage);
                _accumulatedStarvationDamage -= damageToApply;
                
                Health.TakeDamage(damageToApply);
                GodotLogger.SafePrint($"Starvation damage applied: {damageToApply}, Health: {Health.CurrentHealth}/{Health.MaxHealth}");
            }
        }

        private void OnHungerChanged(int currentHunger)
        {
            // Events 버스를 통해 UI에 허기 변경 알림
            var events = EventsHelper.GetEventsNode(this);
            events?.EmitSignal(Events.SignalName.HungerChanged, currentHunger, _hungerComponent.MaxHunger);
        }

        private void OnStarvationStarted()
        {
            // Events 버스를 통해 굶주림 시작 알림
            var events = EventsHelper.GetEventsNode(this);
            events?.EmitSignal(Events.SignalName.StarvationStarted);
            
            GodotLogger.SafePrint("Player is starving! Health will decrease over time.");
        }

        public void EatFood(int foodValue = -1)
        {
            if (foodValue == -1)
            {
                foodValue = GameConstants.Hunger.FOOD_RESTORE_AMOUNT;
            }
            
            _hungerComponent.Eat(foodValue);
            
            // Events 버스를 통해 음식 섭취 알림
            var events = EventsHelper.GetEventsNode(this);
            events?.EmitSignal(Events.SignalName.FoodConsumed);
            
            GodotLogger.SafePrint($"Player consumed food! Hunger restored by {foodValue}");
        }

    }
}