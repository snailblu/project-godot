using Godot;

namespace projectgodot
{
    public partial class Powerup : Area2D
    {
        [Export] public float DamageMultiplier { get; set; } = 2.0f;
        [Export] public float Duration { get; set; } = 5.0f;
        
        private void _on_body_entered(Node2D body)
        {
            if (body is Player player)
            {
                ApplyPowerup(player);
                
                // 파워업 수집 이벤트 발생
                var events = GetNode<Events>("/root/Events");
                events?.EmitSignal(Events.SignalName.PowerupCollected);
                
                // 파워업 아이템 제거
                QueueFree();
            }
        }
        
        private void ApplyPowerup(Player player)
        {
            // Player의 ApplyPowerup 메서드 호출
            player.ApplyPowerup(DamageMultiplier, Duration);
        }
    }
}