using Godot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class Zombie : CharacterBody2D
    {
        [Export] public float MoveSpeed { get; set; } = GameConstants.Zombie.DEFAULT_MOVE_SPEED;
        [Export] public int InitialHealth { get; set; } = GameConstants.Zombie.DEFAULT_INITIAL_HEALTH;
        
        private Player _player;

        // 컴포넌트들을 '부품'처럼 가짐
        public HealthComponent Health { get; private set; }
        private ZombieAIComponent _ai;

        public override void _Ready()
        {
            // Export된 InitialHealth 값으로 체력 컴포넌트 초기화
            Health = new HealthComponent(InitialHealth);
            Health.Died += OnDied;
            Health.HealthChanged += OnHealthChanged;

            // AI 컴포넌트 초기화
            _ai = new ZombieAIComponent();

            // 플레이어 찾기 (더 좋은 방법은 나중에 개선 예정)
            CallDeferred(nameof(FindPlayer));
        }

        private void FindPlayer()
        {
            // Main 씬의 Player 노드를 찾습니다
            var mainScene = GetTree().CurrentScene;
            if (mainScene != null)
            {
                _player = mainScene.GetNodeOrNull<Player>("Player");
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_player == null) return;
            
            if (Health.IsDead) 
            {
                // 안전장치: 죽은 좀비는 즉시 제거
                GD.Print("Dead zombie detected in _PhysicsProcess, removing...");
                QueueFree();
                return;
            }

            // 1. AI 컴포넌트로 방향 계산
            var direction = _ai.CalculateDirection(this.GlobalPosition, _player.GlobalPosition);

            // 2. 이동
            Velocity = direction * MoveSpeed * (float)delta;
            MoveAndSlide();
        }

        private void OnDied()
        {
            GD.Print("Zombie has died!");
            
            // 이벤트 버스를 통해 ZombieDied 이벤트 발생 (100점)
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.ZombieDied, 100);
            
            QueueFree(); // 죽으면 사라짐
        }

        private void OnHealthChanged(int currentHealth)
        {
            // 좀비가 데미지를 받았을 때 사운드 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.ZombieTookDamage);
        }
    }
}