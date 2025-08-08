using Godot;
using System.Threading.Tasks;

namespace projectgodot
{
    public partial class DashComponent : Node
    {
        private DashLogic _dashLogic;
        
        [Export] public float DashSpeed 
        { 
            get => _dashLogic?.DashSpeed ?? 1000f; 
            set { if (_dashLogic != null) _dashLogic.DashSpeed = value; }
        }
        
        [Export] public float DashDuration 
        { 
            get => _dashLogic?.DashDuration ?? 0.2f; 
            set { if (_dashLogic != null) _dashLogic.DashDuration = value; }
        }
        
        public bool IsDashing => _dashLogic?.IsDashing ?? false;

        public override void _Ready()
        {
            _dashLogic = new DashLogic();
        }

        public async void StartDash(Vector2 direction)
        {
            if (_dashLogic == null || IsDashing) return;

            _dashLogic.StartDash();
            
            // Godot의 비동기 타이머를 사용하여 대시 지속시간 처리
            var timer = GetTree()?.CreateTimer(DashDuration);
            if (timer != null)
            {
                await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
            }
            
            // 타이머 종료 시 대시 강제 종료 (안전장치)
            _dashLogic?.EndDash();
        }

        public override void _PhysicsProcess(double delta)
        {
            // 매 프레임 대시 로직 업데이트
            _dashLogic?.Update((float)delta);
        }
    }
}