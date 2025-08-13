using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class MovingState : BasePlayerState
    {
        public override PlayerState StateName => PlayerState.Moving;

        public MovingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnUpdate(double delta)
        {
            // Moving 상태에서는 이동 애니메이션 등을 처리할 수 있음
            // 실제 이동 로직은 PlayerMovement 컴포넌트에서 처리
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // TakingDamage, Dead를 제외하고는 Moving으로 전환 가능
            return fromState != PlayerState.Dead && 
                   fromState != PlayerState.TakingDamage;
        }

        public override bool CanPerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "move":
                case "shoot":
                case "takedamage":
                    return true; // Moving 상태에서는 모든 액션 가능
                default:
                    return false;
            }
        }

        public override StateTransitionRequest CheckTransitionConditions(bool hasMovementInput)
        {
            // Moving에서 Idle로 전환
            if (!hasMovementInput)
            {
                return new StateTransitionRequest(PlayerState.Idle, "No movement input");
            }
            return null;
        }
    }
}