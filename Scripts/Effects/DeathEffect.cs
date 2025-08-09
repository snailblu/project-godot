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
            particles = GetNode<GpuParticles2D>("GPUParticles2D");
            if (particles == null)
            {
                particles = new GpuParticles2D();
                AddChild(particles);
            }

            SetupParticlesByType();
            
            particles.Finished += QueueFree;
            particles.Restart();
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
            
            // 추가 시각 효과 설정
            material.Direction = Vector3.Up; // 위쪽으로 분사
            material.Spread = GameConstants.DeathEffect.SPREAD_ANGLE;
            material.Gravity = Vector3.Down * GameConstants.DeathEffect.GRAVITY_STRENGTH;
        }
    }

    public enum ZombieType
    {
        Basic,
        Runner,
        Tank
    }
}