using Godot;

namespace projectgodot
{
    public partial class Projectile : Area2D
    {
        public Vector2 Direction { get; set; } = Vector2.Up;
        public float Speed { get; set; } = 600.0f;
        public int Damage { get; set; } = 10;

        public override void _Ready()
        {
            // body_entered 시그널을 OnBodyEntered 메서드에 연결
            this.BodyEntered += OnBodyEntered;
            
            // Area2D 속성 명시적 설정
            this.Monitoring = true;
            this.Monitorable = true;
        }

        public override void _PhysicsProcess(double delta)
        {
            Position += Direction * Speed * (float)delta;
        }

        private void OnBodyEntered(Node2D body)
        {
            // 충돌 위치에 파티클 효과 생성
            var hitEffectScene = GD.Load<PackedScene>("res://Scenes/Effects/HitEffect.tscn");
            var hitEffect = hitEffectScene.Instantiate<Node2D>();
            GetTree().CurrentScene.AddChild(hitEffect);
            hitEffect.GlobalPosition = GlobalPosition;
            
            if (body is Zombie zombie)
            {
                zombie.Health.TakeDamage(Damage);
                QueueFree();
            }
            else
            {
                // 좀비가 아닌 대상(벽 등)에 부딪혔을 때 사운드 이벤트 발생
                var events = GetNode<Events>("/root/Events");
                events.EmitSignal(Events.SignalName.ProjectileHitWall);
                QueueFree();
            }
        }
    }
}