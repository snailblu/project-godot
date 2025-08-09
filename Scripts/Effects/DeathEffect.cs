using Godot;
using projectgodot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class DeathEffect : Node2D
    {
        private GpuParticles2D particles;
        public ZombieType ZombieType { get; set; } = ZombieType.Basic;

        public override void _Ready()
        {
            // GPUParticles2D를 직접 생성
            particles = new GpuParticles2D();
            AddChild(particles);

            SetupParticlesByType();
            
            // Z-Index 설정으로 뒤쪽 레이어에 렌더링
            ZIndex = GameConstants.DeathEffect.Z_INDEX;
            
            // 한 번만 방출되도록 설정
            particles.OneShot = true;
            particles.Emitting = true;
            
            particles.Finished += QueueFree;
        }

        private void SetupParticlesByType()
        {
            var material = particles.ProcessMaterial as ParticleProcessMaterial;
            if (material == null)
            {
                material = new ParticleProcessMaterial();
                particles.ProcessMaterial = material;
            }

            switch (ZombieType)
            {
                case ZombieType.Basic:
                    particles.Amount = GameConstants.DeathEffect.BASIC_PARTICLE_COUNT;
                    material.InitialVelocityMin = 50.0f * GameConstants.DeathEffect.BASIC_VELOCITY_SCALE;
                    material.InitialVelocityMax = 100.0f * GameConstants.DeathEffect.BASIC_VELOCITY_SCALE;
                    material.ScaleMin = GameConstants.DeathEffect.BASIC_SCALE_MIN;
                    material.ScaleMax = GameConstants.DeathEffect.BASIC_SCALE_MAX;
                    material.Color = Colors.Red;
                    break;
                    
                case ZombieType.Runner:
                    particles.Amount = GameConstants.DeathEffect.RUNNER_PARTICLE_COUNT;
                    material.InitialVelocityMin = 50.0f * GameConstants.DeathEffect.RUNNER_VELOCITY_SCALE;
                    material.InitialVelocityMax = 100.0f * GameConstants.DeathEffect.RUNNER_VELOCITY_SCALE;
                    material.ScaleMin = GameConstants.DeathEffect.RUNNER_SCALE_MIN;
                    material.ScaleMax = GameConstants.DeathEffect.RUNNER_SCALE_MAX;
                    material.Color = Colors.Orange;
                    break;
                    
                case ZombieType.Tank:
                    particles.Amount = GameConstants.DeathEffect.TANK_PARTICLE_COUNT;
                    material.InitialVelocityMin = 50.0f * GameConstants.DeathEffect.TANK_VELOCITY_SCALE;
                    material.InitialVelocityMax = 100.0f * GameConstants.DeathEffect.TANK_VELOCITY_SCALE;
                    material.ScaleMin = GameConstants.DeathEffect.TANK_SCALE_MIN;
                    material.ScaleMax = GameConstants.DeathEffect.TANK_SCALE_MAX;
                    material.Color = Colors.DarkRed;
                    break;
            }

            // 공통 파티클 설정
            particles.Lifetime = GameConstants.DeathEffect.EFFECT_DURATION;
            
            // 추가 시각 효과 설정 - 각도 범위로 직접 제어
            material.AngleMin = 80.0f;  // 아래쪽 80도부터
            material.AngleMax = 100.0f; // 아래쪽 100도까지 (순전히 아래쪽만)
            material.Gravity = Vector3.Down * GameConstants.DeathEffect.GRAVITY_STRENGTH;
            material.Damping = new Vector2(GameConstants.DeathEffect.LINEAR_DAMP, GameConstants.DeathEffect.LINEAR_DAMP);
        }
    }

    public enum ZombieType
    {
        Basic,
        Runner,
        Tank
    }
}