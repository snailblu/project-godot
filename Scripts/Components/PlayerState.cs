namespace projectgodot.Components
{
    /// <summary>
    /// 플레이어의 가능한 모든 상태를 정의
    /// 상태 우선순위: Dead > TakingDamage > Shooting > Moving > Idle
    /// </summary>
    public enum PlayerState
    {
        /// <summary>기본 대기 상태</summary>
        Idle = 0,
        
        /// <summary>이동 중인 상태</summary>
        Moving = 1,
        
        /// <summary>발사 중인 상태</summary>
        Shooting = 2,
        
        /// <summary>데미지를 받는 중인 상태</summary>
        TakingDamage = 3,
        
        /// <summary>사망 상태</summary>
        Dead = 4
    }

    /// <summary>
    /// 상태 전환 요청을 위한 이벤트 데이터
    /// </summary>
    public class StateTransitionRequest
    {
        public PlayerState TargetState { get; }
        public object Data { get; }
        public string Reason { get; }

        public StateTransitionRequest(PlayerState targetState, string reason = "", object data = null)
        {
            TargetState = targetState;
            Reason = reason;
            Data = data;
        }
    }
}