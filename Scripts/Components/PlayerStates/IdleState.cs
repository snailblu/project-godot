using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class IdleState : BasePlayerState
    {
        public override PlayerState StateName => PlayerState.Idle;

        public IdleState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnUpdate(double delta)
        {
            // Idle 상태에서는 특별한 로직 없음
            // StateMachine이 다른 컴포넌트들의 요청에 따라 상태를 전환할 것
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // 대부분의 상태에서 Idle로 전환 가능
            return fromState != PlayerState.Dead;
        }
    }
}