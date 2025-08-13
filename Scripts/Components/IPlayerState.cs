using Godot;
using projectgodot.Utils;

namespace projectgodot.Components
{
    /// <summary>
    /// 플레이어 상태의 인터페이스
    /// 각 상태는 진입, 업데이트, 종료 시 동작을 정의
    /// </summary>
    public interface IPlayerState
    {
        /// <summary>상태 이름</summary>
        PlayerState StateName { get; }
        
        /// <summary>상태 진입 시 호출</summary>
        /// <param name="previousState">이전 상태</param>
        /// <param name="data">상태 전환 시 전달된 데이터</param>
        void OnEnter(PlayerState previousState, object data = null);
        
        /// <summary>매 프레임 업데이트</summary>
        /// <param name="delta">프레임 시간</param>
        void OnUpdate(double delta);
        
        /// <summary>상태 종료 시 호출</summary>
        /// <param name="nextState">다음 상태</param>
        void OnExit(PlayerState nextState);
        
        /// <summary>해당 상태로 전환 가능한지 확인</summary>
        /// <param name="fromState">현재 상태</param>
        /// <param name="data">전환 데이터</param>
        /// <returns>전환 가능 여부</returns>
        bool CanTransitionFrom(PlayerState fromState, object data = null);
        
        /// <summary>현재 상태에서 특정 액션이 가능한지 확인</summary>
        /// <param name="action">액션 이름 (move, shoot, takedamage 등)</param>
        /// <returns>액션 가능 여부</returns>
        bool CanPerformAction(string action);
        
        /// <summary>이 상태에서 다른 상태로의 자동 전환 조건을 체크</summary>
        /// <param name="hasMovementInput">이동 입력 여부</param>
        /// <returns>전환 요청, 없으면 null</returns>
        StateTransitionRequest CheckTransitionConditions(bool hasMovementInput);
    }

    /// <summary>
    /// 상태 구현을 위한 기본 추상 클래스
    /// </summary>
    public abstract class BasePlayerState : IPlayerState
    {
        public abstract PlayerState StateName { get; }
        
        protected PlayerStateMachine StateMachine { get; private set; }

        public BasePlayerState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnEnter(PlayerState previousState, object data = null)
        {
            // GodotLogger.SafePrint($"Player state: {previousState} -> {StateName}");
        }

        public abstract void OnUpdate(double delta);

        public virtual void OnExit(PlayerState nextState)
        {
            // 기본적으로 아무것도 하지 않음
        }

        public virtual bool CanTransitionFrom(PlayerState fromState, object data = null)
        {
            // 기본적으로 Dead 상태가 아니면 전환 가능
            return fromState != PlayerState.Dead;
        }

        public virtual bool CanPerformAction(string action)
        {
            // 기본적으로 Dead 상태가 아니면 대부분의 액션 가능
            return StateName != PlayerState.Dead;
        }

        public virtual StateTransitionRequest CheckTransitionConditions(bool hasMovementInput)
        {
            // 기본적으로 자동 전환 없음 (각 상태에서 오버라이드)
            return null;
        }
    }
}