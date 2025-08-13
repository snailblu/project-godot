using Godot;

namespace projectgodot.Components.PlayerStates
{
    public class TakingDamageState : BasePlayerState
    {
        private float _damageDuration = 0.2f; // 데미지 상태 유지 시간
        private float _timer = 0f;

        public override PlayerState StateName => PlayerState.TakingDamage;

        public TakingDamageState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter(PlayerState previousState, object data = null)
        {
            base.OnEnter(previousState, data);
            _timer = _damageDuration;
            
            // 데미지 이펙트, 사운드 등을 여기서 처리할 수 있음
        }

        public override void OnUpdate(double delta)
        {
            _timer -= (float)delta;
            
            // 데미지 상태가 끝나면 적절한 상태로 전환
            if (_timer <= 0f)
            {
                // 플레이어가 죽었는지 먼저 체크
                if (StateMachine.Player.Health.IsDead)
                {
                    StateMachine.RequestStateTransition(PlayerState.Dead, "Player died");
                }
                else
                {
                    var targetState = StateMachine.HasMovementInput ? PlayerState.Moving : PlayerState.Idle;
                    StateMachine.RequestStateTransition(targetState, "Damage recovery completed");
                }
            }
        }

        public override bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // Dead를 제외하고는 모든 상태에서 데미지 상태로 전환 가능
            return fromState != PlayerState.Dead;
        }

        public override bool CanPerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "move":
                case "shoot":
                    return false; // 데미지 받는 중에는 이동이나 발사 불가
                case "takedamage":
                    return true; // 추가 데미지는 받을 수 있음
                default:
                    return false;
            }
        }

        public override StateTransitionRequest CheckTransitionConditions(bool hasMovementInput)
        {
            // TakingDamage 상태는 자체적으로 타이머로 종료됨
            // OnUpdate에서 처리하므로 여기서는 null 반환
            return null;
        }
    }
}