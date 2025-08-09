using Godot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class Projectile : Area2D
    {
        public Vector2 Direction { get; set; } = Vector2.Up;
        public float Speed { get; set; } = GameConstants.Projectile.DEFAULT_SPEED;
        public int Damage { get; set; } = GameConstants.Projectile.DEFAULT_DAMAGE;
        
        #if !TEST_ENVIRONMENT
        private BulletTrailEffect _trailEffect;
#endif

        public override void _Ready()
        {
            // body_entered 시그널을 OnBodyEntered 메서드에 연결
            this.BodyEntered += OnBodyEntered;
            
            // Area2D 속성 명시적 설정
            this.Monitoring = true;
            this.Monitorable = true;
            
            // 총알 트레일 효과 생성
#if !TEST_ENVIRONMENT
            _trailEffect = new BulletTrailEffect();
            AddChild(_trailEffect);
            _trailEffect.GlobalPosition = GlobalPosition;
#endif
        }

        public override void _PhysicsProcess(double delta)
        {
            Position += Direction * Speed * (float)delta;
        }

        private void OnBodyEntered(Node2D body)
        {
            // 트레일 효과 중지
#if !TEST_ENVIRONMENT
            _trailEffect?.StopTrail();
#endif
            
            if (body is Zombie zombie)
            {
                // 좀비 충돌 시 기존 HitEffect 생성
                var hitEffectScene = GD.Load<PackedScene>("res://Scenes/Effects/HitEffect.tscn");
                var hitEffect = hitEffectScene.Instantiate<Node2D>();
                GetTree().CurrentScene.AddChild(hitEffect);
                hitEffect.GlobalPosition = GlobalPosition;
                
                zombie.Health.TakeDamage(Damage);
                QueueFree();
            }
            else
            {
                // 벽 충돌 시 스파크 효과 생성
#if !TEST_ENVIRONMENT
                var wallHitEffect = new WallHitEffect();
                GetTree().CurrentScene.AddChild(wallHitEffect);
                wallHitEffect.GlobalPosition = GlobalPosition;
                wallHitEffect.StartEffect(-Direction);
#endif // 충돌 방향 반대로 스파크
                
                // 좀비가 아닌 대상(벽 등)에 부딪혔을 때 사운드 이벤트 발생
                var events = GetNode<Events>("/root/Events");
                events.EmitSignal(Events.SignalName.ProjectileHitWall);
                QueueFree();
            }
        }
    }
}