using Godot;
using projectgodot.Constants;

namespace projectgodot
{
public partial class MuzzleFlashEffect : Node2D
    {
        public override void _Ready()
        {
            // 테스트 환경에서는 Godot API 호출 방지  
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            // 실제 게임에서는 GpuParticles2D 자식 노드를 생성
            var particles = new GpuParticles2D();
            AddChild(particles);
            
            // 총구 화염 효과 설정
            particles.Amount = GameConstants.BulletEffects.MUZZLE_FLASH_PARTICLE_COUNT;
            particles.Lifetime = GameConstants.BulletEffects.MUZZLE_FLASH_DURATION;
            particles.OneShot = true;
            particles.Emitting = false;
            
            // 파티클 머티리얼 설정
            var material = new ParticleProcessMaterial();
            
            // 방향 설정 (부채꼴 모양으로 발사)
            material.Direction = Vector3.Right;
            material.InitialVelocityMin = GameConstants.BulletEffects.MUZZLE_FLASH_VELOCITY * 0.8f;
            material.InitialVelocityMax = GameConstants.BulletEffects.MUZZLE_FLASH_VELOCITY;
            material.AngularVelocityMin = -180.0f;
            material.AngularVelocityMax = 180.0f;
            
            // 확산 각도 설정
            material.Spread = 30.0f;
            
            // 크기 변화
            material.ScaleMin = GameConstants.BulletEffects.MUZZLE_FLASH_SCALE_MIN;
            material.ScaleMax = GameConstants.BulletEffects.MUZZLE_FLASH_SCALE_MAX;
            
            // 색상 변화 (노란색 -> 빨간색 -> 투명)
            var gradient = new Gradient();
            gradient.AddPoint(0.0f, Colors.Yellow);
            gradient.AddPoint(0.3f, Colors.Orange);
            gradient.AddPoint(0.7f, Colors.Red);
            gradient.AddPoint(1.0f, Colors.Transparent);
            
            var gradientTexture = new GradientTexture1D();
            gradientTexture.Gradient = gradient;
            material.ColorRamp = gradientTexture;
            
            // 감쇠 효과
            material.LinearAccelMin = -50.0f;
            material.LinearAccelMax = -100.0f;
            
            particles.ProcessMaterial = material;
            
            // 파티클 완료 후 자동 제거
            particles.Finished += QueueFree;
        }
        
        public void StartEffect()
        {
            if (EnvironmentHelper.IsTestEnvironment())
                return;
                
            var particles = GetChild<GpuParticles2D>(0);
            if (particles != null)
            {
                particles.Restart();
                particles.Emitting = true;
            }
        }
    }
}