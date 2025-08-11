using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class StarvingState : BasePlayerState
    {
        public override PlayerState StateName => PlayerState.Starving;

        public StarvingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter(PlayerState previousState, object data = null)
        {
            base.OnEnter(previousState, data);
            // 굶주림 시각적 효과 시작 (화면 효과, 사운드 등)
        }

        public override void OnUpdate(double delta)
        {
            // 굶주림 상태에서는 계속해서 체력 감소
            // 실제 로직은 HungerComponent에서 처리
            
            // 플레이어가 죽었는지 체크
            if (StateMachine.Player.Health.IsDead)
            {
                StateMachine.RequestStateTransition(PlayerState.Dead, "Player died from starvation");
                return;
            }
            
            // 더 이상 굶주리지 않으면 다른 상태로 전환
            // (실제로는 CheckAutoStateTransitions에서 처리될 예정)
        }

        public override void OnExit(PlayerState nextState)
        {
            base.OnExit(nextState);
            // 굶주림 시각적 효과 종료
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // Dead를 제외하고는 모든 상태에서 굶주림 상태로 전환 가능
            return fromState != PlayerState.Dead;
        }
    }
}