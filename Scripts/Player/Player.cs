using Godot;
using projectgodot.Components;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class Player : CharacterBody2D
    {
        private PlayerData _playerData;
        private PlayerMovement _playerMovement;
        private WeaponComponent _weapon;
        private DashComponent _dash;
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
        
        public HealthComponent Health { get; private set; }

        public override void _Ready()
        {
            // TDD로 검증된 클래스들을 인스턴스화 (컴포지션 패턴)
            _playerData = new PlayerData();
            _playerMovement = new PlayerMovement();
            Health = new HealthComponent(100);
            _dash = new DashComponent();

            // SceneFactory 찾기 (GameManager에서 제공될 예정)
            // 현재 씬에서 SceneFactory를 동적으로 찾기
            var sceneRoot = GetTree().CurrentScene;
            _sceneFactory = sceneRoot.FindChild("SceneFactory", true) as ISceneFactory;
            
            if (_sceneFactory == null)
            {
                GD.PrintErr("SceneFactory를 찾을 수 없습니다. 현재 씬에서 SceneFactory 노드를 찾을 수 없습니다.");
            }

            // 무기 시스템 초기화 - 권총: 초당 4발 (0.25초 쿨다운)
            _weapon = new WeaponComponent(cooldown: 0.25f);
            _weapon.OnShoot += SpawnProjectile;
            _originalWeaponDamage = _weapon.Damage;
            
            // 파워업 로직 초기화
            _powerupLogic = new PowerupLogic();

            // 발사체 씬 로드
            _projectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/projectile.tscn");

            // 사망 이벤트 연결
            Health.Died += OnDeath;
            
            // 체력 변경 이벤트를 전역 이벤트 버스로 전달
            Health.HealthChanged += OnHealthChanged;
            
            // DashComponent를 자식 노드로 추가
            AddChild(_dash);
            
            // CameraShakeComponent 추가
            _cameraShake = new CameraShakeComponent();
            AddChild(_cameraShake);
            
            // 새로운 컴포넌트들 초기화
            _inputHandler = new PlayerInputHandler();
            _animationController = new PlayerAnimationController();
            _collisionHandler = new PlayerCollisionHandler();
            _eventBridge = new PlayerEventBridge();
            
            AddChild(_inputHandler);
            AddChild(_animationController);
            AddChild(_collisionHandler);
            AddChild(_eventBridge);
            
            // 컴포넌트들 초기화 및 이벤트 연결
            _collisionHandler.Initialize(_dash);
            
            // 이벤트 연결
            _inputHandler.MovementRequested += OnMovementRequested;
            _inputHandler.DashRequested += OnDashRequested;
            _inputHandler.ShootRequested += OnShootRequested;
            _inputHandler.TestDamageRequested += OnTestDamageRequested;
            _collisionHandler.DamageReceived += OnDamageReceived;
            
            // 카메라 쉐이크 이벤트 연결
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
            {
                events.CameraShakeRequested += OnCameraShakeRequested;
            }
        }

        private Vector2 _currentMovementDirection;

        public override void _PhysicsProcess(double delta)
        {
            // 대시 상태 업데이트
            _inputHandler.SetDashingState(_dash.IsDashing);
            
            // TDD로 검증된 로직 클래스를 이용해 속도 계산
            Vector2 calculatedVelocity;
            
            if (_dash.IsDashing)
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

            // Godot 노드에 결과 적용
            Velocity = calculatedVelocity;
            MoveAndSlide();

            // 애니메이션 업데이트
            _animationController.UpdateAnimation(Velocity);

            // 무기 쿨다운 업데이트
            _weapon.Process((float)delta);
            
            // 파워업 효과 업데이트
            UpdatePowerupEffect();
        }

        // 입력 이벤트 핸들러들
        private void OnMovementRequested(Vector2 direction)
        {
            _currentMovementDirection = direction;
        }

        private void OnDashRequested(Vector2 direction)
        {
            _dash.StartDash(direction);
            _eventBridge.RequestDashShake();
        }

        private void OnShootRequested()
        {
            _weapon.Shoot();
        }

        private void OnTestDamageRequested()
        {
            Health.TakeDamage(10);
            GD.Print($"Player took 10 damage! Current health: {Health.CurrentHealth}/{Health.MaxHealth}");
            _eventBridge.RequestHeavyShake();
        }

        private void OnDamageReceived(int damage)
        {
            Health.TakeDamage(damage);
            GD.Print($"Current health: {Health.CurrentHealth}/{Health.MaxHealth}");
            _eventBridge.RequestHeavyShake();
        }

        private void OnDeath()
        {
            GD.Print("Player has died!");
            _eventBridge.NotifyGameOver();
            QueueFree();
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

        private void OnBodyEntered(Node2D body)
        {
            // 부딪힌 대상이 Zombie 클래스인지 확인
            if (body is Zombie)
            {
                // 대시 중에는 무적 상태
                if (_dash.IsDashing)
                {
                    GD.Print("Player is dashing - immune to damage!");
                    return;
                }
                
                // 체력 10 감소
                Health.TakeDamage(10);
                GD.Print($"Player took 10 damage from zombie! Current health: {Health.CurrentHealth}/{Health.MaxHealth}");
                
                // 데미지를 받을 때 강한 카메라 쉐이크
                EventsHelper.EmitSignalSafe(this, Events.SignalName.CameraShakeRequested, 
                    GameConstants.CameraShake.HEAVY_INTENSITY, 
                    GameConstants.CameraShake.HEAVY_DURATION);
            }
        }

        private void UpdatePowerupEffect()
        {
            _powerupLogic.Update();
            
            // 파워업 효과가 방금 종료되었는지 확인
            if (!_powerupLogic.IsActive && _weapon.Damage != _originalWeaponDamage)
            {
                // 원래 데미지로 복구
                _weapon.Damage = _originalWeaponDamage;
                GD.Print($"Powerup effect ended! Damage restored to {_originalWeaponDamage}");
            }
        }
        
        public void ApplyPowerup(float damageMultiplier, float duration)
        {
            if (_powerupLogic.IsActive) 
            {
                GD.Print("Powerup already active, ignoring new one");
                return;
            }
            
            _powerupLogic.DamageMultiplier = damageMultiplier;
            _powerupLogic.Duration = duration;
            _powerupLogic.Activate(_originalWeaponDamage);
            
            var newDamage = _powerupLogic.CalculateDamage(_originalWeaponDamage);
            _weapon.Damage = newDamage;
            
            GD.Print($"Powerup applied! Damage: {_originalWeaponDamage} -> {newDamage} for {duration} seconds");
        }

        private void OnCameraShakeRequested(float intensity, float duration)
        {
            _cameraShake?.StartShake(intensity, duration);
        }

        private void OnHealthChanged(int currentHealth)
        {
            _eventBridge.NotifyPlayerHealthChanged(currentHealth, Health.MaxHealth);
        }

    }
}