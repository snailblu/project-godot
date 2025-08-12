using Godot;
using System;
using projectgodot.Components;

namespace projectgodot.Components
{
    public partial class PlayerCollisionHandler : Node
    {
        // 충돌 이벤트
        public event Action<int> DamageReceived;

        private PlayerStateMachine _stateMachine;
        private const int ZOMBIE_DAMAGE = 10;

        public void Initialize(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void HandleBodyEntered(Node2D body)
        {
            // 부딪힌 대상이 Zombie 클래스인지 확인
            if (body is Zombie)
            {
                
                // 체력 10 감소 이벤트 발생
                DamageReceived?.Invoke(ZOMBIE_DAMAGE);
                GD.Print($"Player took {ZOMBIE_DAMAGE} damage from zombie!");
            }
        }
    }
}