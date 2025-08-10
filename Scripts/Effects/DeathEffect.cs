using Godot;
using projectgodot;

namespace projectgodot
{
    public partial class DeathEffect : Node2D
    {
        private GpuParticles2D particles;

        public override void _Ready()
        {
            // 씬에서 미리 설정된 GPUParticles2D 가져오기
            particles = GetNode<GpuParticles2D>("GPUParticles2D");
            
            // 파티클 시작
            particles.Emitting = true;
            
            particles.Finished += QueueFree;
        }
    }

    public enum ZombieType
    {
        Basic,
        Runner,
        Tank
    }
}