using Godot;

namespace projectgodot
{
    public partial class HitEffect : Node2D
    {
        private GpuParticles2D particles;

        public override void _Ready()
        {
            // 파티클 노드가 자식으로 있는지 확인하고, 없으면 생성
            particles = GetNode<GpuParticles2D>("GPUParticles2D");
            if (particles == null)
            {
                particles = new GpuParticles2D();
                AddChild(particles);
            }

            // 파티클이 완료되면 자동으로 제거
            particles.Finished += QueueFree;
            
            // 파티클 재생 시작
            particles.Restart();
            
        }
    }
}