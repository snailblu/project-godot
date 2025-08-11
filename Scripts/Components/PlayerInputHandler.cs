using Godot;
using System;
using projectgodot.Components;
using projectgodot.Constants;

namespace projectgodot.Components
{
    public partial class PlayerInputHandler : Node
    {
        // 입력 이벤트들
        public event Action<Vector2> MovementRequested;
        public event Action<Vector2> DashRequested;
        public event Action ShootRequested;
        public event Action TestDamageRequested;
        public event Action EatFoodRequested;

        private bool _isDashing = false;

        public void SetDashingState(bool isDashing)
        {
            _isDashing = isDashing;
        }

        public override void _PhysicsProcess(double delta)
        {
            // 1. 이동 입력 받기
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            MovementRequested?.Invoke(direction);
            
            // 2. 대시 입력 처리 (Shift 키)
            if (Input.IsActionJustPressed("dash") && !_isDashing)
            {
                if (direction != Vector2.Zero)
                {
                    DashRequested?.Invoke(direction);
                }
            }

            // 3. 임시 데미지 테스트 (T키)
            if (Input.IsActionJustPressed("ui_accept"))
            {
                TestDamageRequested?.Invoke();
            }
        }

        // 음식 섭취 입력을 _Input 메서드에 추가
        public void HandleEatFoodInput()
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                EatFoodRequested?.Invoke();
            }
        }

        public override void _Input(InputEvent @event)
        {
            // 마우스 왼쪽 버튼을 누르고 있으면 발사 요청
            if (Input.IsActionPressed("shoot"))
            {
                ShootRequested?.Invoke();
            }
        }
    }
}