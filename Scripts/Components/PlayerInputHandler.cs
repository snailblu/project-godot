using Godot;
using System;
using projectgodot.Components;
using projectgodot.Constants;
using projectgodot.Scripts.Interfaces;

namespace projectgodot.Components
{
    public partial class PlayerInputHandler : Node, IPlayerInput
    {
        // 입력 이벤트들
        public event Action<Vector2> MovementRequested;
        public event Action ShootRequested;

        private PlayerStateMachine _stateMachine;

        public void Initialize(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void _PhysicsProcess(double delta)
        {
            // 1. 이동 입력 받기
            Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            MovementRequested?.Invoke(direction);
            
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