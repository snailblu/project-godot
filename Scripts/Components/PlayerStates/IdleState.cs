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

        public override bool CanPerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "move":
                case "shoot":
                case "takedamage":
                    return true; // Idle 상태에서는 모든 액션 가능
                default:
                    return false;
            }
        }

        public override StateTransitionRequest CheckTransitionConditions(bool hasMovementInput)
        {
            // Idle에서 Moving으로 전환
            if (hasMovementInput)
            {
                return new StateTransitionRequest(PlayerState.Moving, "Movement input detected");
            }
            return null;
        }
    }
}