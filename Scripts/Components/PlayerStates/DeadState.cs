using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class DeadState : BasePlayerState
    {
        public override PlayerState StateName => PlayerState.Dead;

        public DeadState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter(PlayerState previousState, object data = null)
        {
            base.OnEnter(previousState, data);
            // 사망 애니메이션, 사운드, 게임 오버 처리 등
        }

        public override void OnUpdate(double delta)
        {
            // Dead 상태에서는 아무것도 하지 않음
            // 게임 오버 처리는 이미 OnEnter에서 수행됨
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // 모든 상태에서 Dead로 전환 가능
            return true;
        }

        public override bool CanPerformAction(string action)
        {
            // Dead 상태에서는 모든 액션 불가능
            return false;
        }

        public override StateTransitionRequest CheckTransitionConditions(bool hasMovementInput)
        {
            // Dead 상태에서는 다른 상태로 전환 불가
            return null;
        }
    }
}