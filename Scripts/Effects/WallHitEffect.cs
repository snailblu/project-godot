using Godot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class WallHitEffect : Node2D
    {
        private GpuParticles2D _particles;
        
        public override void _Ready()
        {
            // 테스트 환경에서는 Godot API 호출 방지
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            // 실제 게임에서는 GpuParticles2D 자식 노드를 생성
            _particles = new GpuParticles2D();
            AddChild(_particles);
                
            // 벽 충돌 스파크 효과 설정
            _particles.Amount = GameConstants.BulletEffects.WALL_HIT_SPARK_COUNT;
            _particles.Lifetime = GameConstants.BulletEffects.SPARK_DURATION;
            _particles.OneShot = true;
            _particles.Emitting = false;
            
            // 파티클 머티리얼 설정
            var material = new ParticleProcessMaterial();
            
            // 스파크가 모든 방향으로 튀도록 설정
            material.Direction = Vector3.Up;
            material.InitialVelocityMin = GameConstants.BulletEffects.SPARK_VELOCITY_MIN;
            material.InitialVelocityMax = GameConstants.BulletEffects.SPARK_VELOCITY_MAX;
            
            // 넓은 확산 각도 (반구 모양)
            material.Spread = 90.0f;
            material.Flatness = 0.0f;
            
            // 회전 효과
            material.AngularVelocityMin = -360.0f;
            material.AngularVelocityMax = 360.0f;
            
            // 크기 설정
            material.ScaleMin = GameConstants.BulletEffects.SPARK_SCALE_MIN;
            material.ScaleMax = GameConstants.BulletEffects.SPARK_SCALE_MAX;
            
            // 색상 변화 (밝은 흰색 -> 노란색 -> 주황색 -> 투명)
            var gradient = new Gradient();
            gradient.AddPoint(0.0f, Colors.White);
            gradient.AddPoint(0.1f, Colors.Yellow);
            gradient.AddPoint(0.4f, Colors.Orange);
            gradient.AddPoint(0.8f, Colors.Red);
            gradient.AddPoint(1.0f, Colors.Transparent);
            
            var gradientTexture = new GradientTexture1D();
            gradientTexture.Gradient = gradient;
            material.ColorRamp = gradientTexture;
            
            // 중력 효과 (스파크가 아래로 떨어지도록)
            material.Gravity = new Vector3(0, GameConstants.BulletEffects.SPARK_GRAVITY, 0);
            
            // 감쇠 효과 (마찰력)
            material.LinearAccelMin = -30.0f;
            material.LinearAccelMax = -60.0f;
            
            _particles.ProcessMaterial = material;
            
            // 파티클 완료 후 자동 제거
            _particles.Finished += QueueFree;
        }
        
        public void StartEffect(Vector2 impactDirection)
        {
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            // 충돌 방향에 따라 스파크 방향 조정
            var material = _particles.ProcessMaterial as ParticleProcessMaterial;
            if (material != null && impactDirection != Vector2.Zero)
            {
                // 충돌 방향의 반대쪽으로 스파크가 더 많이 튀도록
                var reflectDirection = impactDirection.Bounce(Vector2.Up);
                material.Direction = new Vector3(reflectDirection.X, reflectDirection.Y, 0);
            }
            
            _particles.Restart();
            _particles.Emitting = true;
        }
        
        public void StartEffect()
        {
            // 기본 방향으로 스파크 효과 시작
            StartEffect(Vector2.Zero);
        }
    }
}