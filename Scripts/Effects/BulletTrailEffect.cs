using Godot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class BulletTrailEffect : Node2D
    {
        private Vector2 _previousPosition;
        private Vector2 _direction;
        
        public override void _Ready()
        {
            // 테스트 환경에서는 Godot API 호출 방지
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            // 실제 게임에서는 GpuParticles2D 자식 노드를 생성
            var particles = new GpuParticles2D();
            AddChild(particles);
            
            // 총알 트레일 효과 설정
            particles.Amount = GameConstants.BulletEffects.BULLET_TRAIL_PARTICLE_COUNT;
            particles.Lifetime = GameConstants.BulletEffects.TRAIL_LIFETIME;
            particles.Emitting = true;
            
            // 파티클 머티리얼 설정
            var material = new ParticleProcessMaterial();
            
            // 트레일이 총알 뒤로 나오도록 설정
            material.Direction = Vector3.Back;
            material.InitialVelocityMin = 10.0f;
            material.InitialVelocityMax = 30.0f;
            
            // 약간의 확산
            material.Spread = 5.0f;
            
            // 크기 설정 (작게 시작해서 더 작아짐)
            material.ScaleMin = 0.3f;
            material.ScaleMax = 0.8f;
            
            // 색상 변화 (밝은 노란색 -> 주황색 -> 투명)
            var gradient = new Gradient();
            gradient.AddPoint(0.0f, Colors.White);
            gradient.AddPoint(0.2f, Colors.Yellow);
            gradient.AddPoint(0.6f, Colors.Orange);
            gradient.AddPoint(1.0f, Colors.Transparent);
            
            var gradientTexture = new GradientTexture1D();
            gradientTexture.Gradient = gradient;
            material.ColorRamp = gradientTexture;
            
            // 중력 효과 없음 (수평으로 이동하는 트레일)
            material.Gravity = Vector3.Zero;
            
            // 감쇠 효과
            material.LinearAccelMin = -20.0f;
            material.LinearAccelMax = -40.0f;
            
            particles.ProcessMaterial = material;
            
            _previousPosition = GlobalPosition;
        }
        
        public override void _Process(double delta)
        {
            // 테스트 환경에서는 Godot API 호출 방지
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            // 총알의 이동 방향 계산
            Vector2 currentPosition = GlobalPosition;
            _direction = (currentPosition - _previousPosition).Normalized();
            
            // 트레일이 총알 이동 방향 반대로 나오도록 조정
            if (_direction != Vector2.Zero)
            {
                var particles = GetChild<GpuParticles2D>(0);
                if (particles?.ProcessMaterial is ParticleProcessMaterial material)
                {
                    // 총알 이동 방향의 반대로 파티클 발사
                    material.Direction = new Vector3(-_direction.X, -_direction.Y, 0);
                }
            }
            
            _previousPosition = currentPosition;
        }
        
        public void StopTrail()
        {
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            var particles = GetChild<GpuParticles2D>(0);
            if (particles != null)
            {
                particles.Emitting = false;
                
                // 파티클이 모두 사라질 때까지 기다린 후 제거
                var timer = GetTree().CreateTimer(particles.Lifetime);
                timer.Timeout += QueueFree;
            }
        }
    }
}