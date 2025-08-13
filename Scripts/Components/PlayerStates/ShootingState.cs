using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class ShootingState : BasePlayerState
    {
        private float _shootingDuration = 0.1f; // 발사 상태 유지 시간
        private float _timer = 0f;

        public override PlayerState StateName => PlayerState.Shooting;

        public ShootingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter(PlayerState previousState, object data = null)
        {
            base.OnEnter(previousState, data);
            _timer = _shootingDuration;
        }

        public override void OnUpdate(double delta)
        {
            _timer -= (float)delta;
            
            // 발사 상태가 끝나면 이전 상태로 복귀 (Moving 또는 Idle)
            if (_timer <= 0f)
            {
                // 움직임이 있으면 Moving, 없으면 Idle로 전환
                var targetState = StateMachine.HasMovementInput ? PlayerState.Moving : PlayerState.Idle;
                StateMachine.RequestStateTransition(targetState, "Shooting completed");
            }
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // Dead, TakingDamage 상태가 아니면 발사 가능
            return fromState != PlayerState.Dead && 
                   fromState != PlayerState.TakingDamage;
        }

        public override bool CanPerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "move":
                    return true; // 발사하면서도 이동 가능
                case "shoot":
                    return false; // 이미 발사 중이므로 추가 발사 불가
                case "takedamage":
                    return true; // 발사 중에도 데미지 받을 수 있음
                default:
                    return false;
            }
        }

        public override StateTransitionRequest CheckTransitionConditions(bool hasMovementInput)
        {
            // Shooting 상태는 자체적으로 타이머로 종료됨
            // OnUpdate에서 처리하므로 여기서는 null 반환
            return null;
        }
    }
}