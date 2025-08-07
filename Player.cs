using Godot;

namespace projectgodot
{
    public partial class Player : CharacterBody2D
    {
        private PlayerData _playerData;
        private PlayerMovement _playerMovement;
        private WeaponComponent _weapon;
        private PackedScene _projectileScene;
        private ISceneFactory _sceneFactory;
        public HealthComponent Health { get; private set; }

        public override void _Ready()
        {
            // TDD로 검증된 클래스들을 인스턴스화 (컴포지션 패턴)
            _playerData = new PlayerData();
            _playerMovement = new PlayerMovement();
            Health = new HealthComponent(100);

            // SceneFactory 찾기 (GameManager에서 제공될 예정)
            _sceneFactory = GetNode<SceneFactory>("/root/Main/GameManager/SceneFactory");

            // 무기 시스템 초기화 - 권총: 초당 4발 (0.25초 쿨다운)
            _weapon = new WeaponComponent(cooldown: 0.25f);
            _weapon.OnShoot += SpawnProjectile;

            // 발사체 씬 로드
            _projectileScene = GD.Load<PackedScene>("res://projectile.tscn");

            // 사망 이벤트 연결
            Health.Died += OnDeath;
            
            // 체력 변경 이벤트를 전역 이벤트 버스로 전달
            Health.HealthChanged += OnHealthChanged;
        }

        public override void _PhysicsProcess(double delta)
        {
            // 1. 입력 받기
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            // 임시 데미지 테스트 (T키)
            if (Input.IsActionJustPressed("ui_accept")) // T키는 기본적으로 ui_accept에 매핑됨
            {
                Health.TakeDamage(10);
                GD.Print($"Player took 10 damage! Current health: {Health.CurrentHealth}/{Health.MaxHealth}");
            }

            // 2. TDD로 검증된 로직 클래스를 이용해 속도 계산
            Vector2 calculatedVelocity = _playerMovement.CalculateVelocity(
                direction, 
                _playerData.MovementSpeed, 
                (float)delta
            );

            // 3. Godot 노드에 결과 적용
            Velocity = calculatedVelocity;
            MoveAndSlide();

            // 4. 무기 쿨다운 업데이트
            _weapon.Process((float)delta);
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
            QueueFree(); // 씬 트리에서 노드를 제거
        }

        private void SpawnProjectile()
        {
            if (_sceneFactory == null) return;
            
            // SceneFactory를 통해 발사체 생성
            var direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
            _sceneFactory.CreateProjectile(_projectileScene, GlobalPosition, direction);
        }

        private void OnBodyEntered(Node2D body)
        {
            // 부딪힌 대상이 Zombie 클래스인지 확인
            if (body is Zombie)
            {
                // 체력 10 감소
                Health.TakeDamage(10);
                GD.Print($"Player took 10 damage from zombie! Current health: {Health.CurrentHealth}/{Health.MaxHealth}");
            }
        }

        private void OnHealthChanged(int currentHealth)
        {
            // 체력 변경을 이벤트 버스를 통해 전달
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.PlayerHealthChanged, currentHealth, Health.MaxHealth);
        }
    }
}