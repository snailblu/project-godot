using Godot;

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
        }

        public override void _PhysicsProcess(double delta)
        {
            // 1. 입력 받기
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            
            // 대시 입력 처리 (Shift 키)
            if (Input.IsActionJustPressed("dash") && !_dash.IsDashing)
            {
                if (direction != Vector2.Zero)
                {
                    _dash.StartDash(direction);
                }
            }

            // 임시 데미지 테스트 (T키)
            if (Input.IsActionJustPressed("ui_accept")) // T키는 기본적으로 ui_accept에 매핑됨
            {
                Health.TakeDamage(10);
                GD.Print($"Player took 10 damage! Current health: {Health.CurrentHealth}/{Health.MaxHealth}");
            }

            // 2. TDD로 검증된 로직 클래스를 이용해 속도 계산
            Vector2 calculatedVelocity;
            
            if (_dash.IsDashing)
            {
                // 대시 중일 때는 대시 속도 사용
                calculatedVelocity = direction * _dash.DashSpeed;
            }
            else
            {
                // 일반 이동
                calculatedVelocity = _playerMovement.CalculateVelocity(
                    direction, 
                    _playerData.MovementSpeed, 
                    (float)delta
                );
            }

            // 3. Godot 노드에 결과 적용
            Velocity = calculatedVelocity;
            MoveAndSlide();

            // 4. 무기 쿨다운 업데이트
            _weapon.Process((float)delta);
            
            // 5. 파워업 효과 업데이트
            UpdatePowerupEffect();
        }

        public override void _Input(InputEvent @event)
        {
            // 마우스 왼쪽 버튼을 누르고 있으면 발사 시도
            if (Input.IsActionPressed("shoot"))
            {
                _weapon.Shoot();
            }
        }

        private void OnDeath()
        {
            GD.Print("Player has died!");
            
            // 게임 오버 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.GameOver);
            
            QueueFree(); // 씬 트리에서 노드를 제거
        }

        private void SpawnProjectile()
        {
            if (_sceneFactory == null) return;
            
            // Events 싱글톤을 통해 무기 발사 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.PlayerFiredWeapon);
            
            // SceneFactory를 통해 발사체 생성
            var direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
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

        private void OnHealthChanged(int currentHealth)
        {
            // 체력 변경을 이벤트 버스를 통해 전달
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.PlayerHealthChanged, currentHealth, Health.MaxHealth);
            
            // 체력이 감소했을 때 (데미지를 받았을 때) 사운드 이벤트 발생
            if (currentHealth < Health.MaxHealth)
            {
                events.EmitSignal(Events.SignalName.PlayerTookDamage);
            }
        }
    }
}