using Godot;

namespace projectgodot
{
    public partial class Zombie : CharacterBody2D
    {
        public const float Speed = 900.0f;
        private Player _player;

        // 컴포넌트들을 '부품'처럼 가짐
        public HealthComponent Health { get; private set; }
        private ZombieAIComponent _ai;

        public override void _Ready()
        {
            // 좀비는 체력 30으로 시작
            Health = new HealthComponent(30);
            Health.Died += () => {
                GD.Print("Zombie has died!");
                QueueFree(); // 죽으면 사라짐
            };

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
            Velocity = direction * Speed * (float)delta;
            MoveAndSlide();
        }
    }
}