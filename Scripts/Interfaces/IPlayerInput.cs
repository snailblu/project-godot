using System;
using Godot;
using projectgodot.Components;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 플레이어 입력 처리를 담당하는 인터페이스
    /// 사용자 입력을 받아 적절한 이벤트를 발생시킴
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>
        /// StateMachine 초기화
        /// </summary>
        /// <param name="stateMachine">플레이어 상태 머신</param>
        void Initialize(PlayerStateMachine stateMachine);
                
        /// <summary>
        /// 이동 요청 이벤트
        /// </summary>
        event Action<Vector2> MovementRequested;
        
        
        /// <summary>
        /// 발사 요청 이벤트
        /// </summary>
        event Action ShootRequested;
        
    }
}