using Godot;
using projectgodot.Components;
using projectgodot.Constants;

namespace projectgodot.Components
{
    public partial class PlayerEventBridge : Node
    {
        private Events _events;

        public override void _Ready()
        {
            _events = GetNode<Events>("/root/Events");
        }

        // 카메라 쉐이크 이벤트들
        public void RequestDashShake()
        {
            _events?.EmitSignal(Events.SignalName.CameraShakeRequested, 
                GameConstants.CameraShake.DASH_INTENSITY, 
                GameConstants.CameraShake.DASH_DURATION);
        }

        public void RequestHeavyShake()
        {
            _events?.EmitSignal(Events.SignalName.CameraShakeRequested, 
                GameConstants.CameraShake.HEAVY_INTENSITY, 
                GameConstants.CameraShake.HEAVY_DURATION);
        }

        public void RequestLightShake()
        {
            _events?.EmitSignal(Events.SignalName.CameraShakeRequested, 
                GameConstants.CameraShake.LIGHT_INTENSITY, 
                GameConstants.CameraShake.LIGHT_DURATION);
        }

        // 게임 이벤트들
        public void NotifyGameOver()
        {
            _events?.EmitSignal(Events.SignalName.GameOver);
        }

        public void NotifyPlayerFiredWeapon()
        {
            _events?.EmitSignal(Events.SignalName.PlayerFiredWeapon);
        }

        public void NotifyPlayerHealthChanged(int currentHealth, int maxHealth)
        {
            _events?.EmitSignal(Events.SignalName.PlayerHealthChanged, currentHealth, maxHealth);
            
            // 체력이 감소했을 때 (데미지를 받았을 때) 사운드 이벤트 발생
            if (currentHealth < maxHealth)
            {
                _events?.EmitSignal(Events.SignalName.PlayerTookDamage);
            }
        }
    }
}