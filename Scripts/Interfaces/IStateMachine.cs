using System;
using projectgodot.Components;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 플레이어 상태 머신을 담당하는 인터페이스
    /// 플레이어의 다양한 상태와 상태 전환을 관리
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// 현재 상태
        /// </summary>
        PlayerState CurrentState { get; }
        
        /// <summary>
        /// 움직임 입력이 있는지 여부
        /// </summary>
        bool HasMovementInput { get; }
        
        /// <summary>
        /// 특정 상태인지 확인
        /// </summary>
        /// <param name="state">확인할 상태</param>
        /// <returns>해당 상태인지 여부</returns>
        bool IsInState(PlayerState state);
        
        /// <summary>
        /// 특정 액션을 수행할 수 있는지 확인
        /// </summary>
        /// <param name="action">액션 이름</param>
        /// <returns>액션 수행 가능 여부</returns>
        bool CanPerformAction(string action);
        
        /// <summary>
        /// 이동 입력 상태 업데이트
        /// </summary>
        /// <param name="hasMovement">이동 입력 여부</param>
        void UpdateMovementInput(bool hasMovement);
        
        
        /// <summary>
        /// 발사 요청
        /// </summary>
        /// <returns>발사 가능 여부</returns>
        bool RequestShoot();
        
        /// <summary>
        /// 데미지 받음 요청
        /// </summary>
        /// <returns>데미지 상태 전환 가능 여부</returns>
        bool RequestTakeDamage();
        
        /// <summary>
        /// 상태 변경 이벤트
        /// </summary>
        event Action<PlayerState, PlayerState> StateChanged;
    }
}