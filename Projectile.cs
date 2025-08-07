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
            if (body is Zombie zombie)
            {
                zombie.Health.TakeDamage(Damage);
                QueueFree();
            }
        }
    }
}