using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class DashingState : BasePlayerState
    {
        public override PlayerState StateName => PlayerState.Dashing;

        public DashingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter(PlayerState previousState, object data = null)
        {
            base.OnEnter(previousState, data);
            // 대시 시작 시 무적 상태 설정 등의 로직이 여기에 올 수 있음
        }

        public override void OnUpdate(double delta)
        {
            // DashComponent에서 대시가 끝났는지 체크하여 상태 전환 요청
            // 실제 대시 로직은 DashComponent와 DashLogic에서 처리
        }

        public override void OnExit(PlayerState nextState)
        {
            base.OnExit(nextState);
            // 대시 종료 시 무적 해제 등
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // Dead, TakingDamage 상태가 아니면 대시 가능
            return fromState != PlayerState.Dead && 
                   fromState != PlayerState.TakingDamage;
        }
    }
}