using System.Collections.Generic;
using Godot;

namespace projectgodot.Components
{
    /// <summary>
    /// 상태 전환 우선순위를 관리하는 클래스
    /// PlayerStateMachine의 복잡한 우선순위 로직을 분리하여 단순화
    /// </summary>
    public class StateTransitionPriority
    {
        private readonly Player _player;
        
        // 우선순위 순서로 정렬된 상태 체크 목록
        // Dead > TakingDamage > Shooting > Moving > Idle
        private readonly List<PlayerState> _priorityOrder = new List<PlayerState>
        {
            PlayerState.Dead,
            PlayerState.TakingDamage,
            PlayerState.Shooting,
            PlayerState.Moving,
            PlayerState.Idle
        };

        public StateTransitionPriority(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// 자동 상태 전환을 위한 다음 상태를 결정
        /// </summary>
        /// <param name="currentState">현재 상태</param>
        /// <param name="hasMovementInput">이동 입력 여부</param>
        /// <returns>전환해야 할 상태와 이유, 전환이 필요 없으면 null</returns>
        public StateTransitionRequest GetAutoTransition(PlayerState currentState, bool hasMovementInput)
        {
            if (_player == null) return null;

            // 최고 우선순위: Dead
            if (_player.Health.IsDead && currentState != PlayerState.Dead)
            {
                return new StateTransitionRequest(PlayerState.Dead, "Player health reached zero");
            }

            // Dead 상태면 더 이상 체크하지 않음
            if (currentState == PlayerState.Dead) return null;


            // Moving/Idle 상태 간 전환 (낮은 우선순위)
            if (currentState == PlayerState.Idle && hasMovementInput)
            {
                return new StateTransitionRequest(PlayerState.Moving, "Movement input detected");
            }
            else if (currentState == PlayerState.Moving && !hasMovementInput)
            {
                return new StateTransitionRequest(PlayerState.Idle, "No movement input");
            }

            return null; // 전환이 필요하지 않음
        }

        /// <summary>
        /// 특정 상태의 우선순위를 반환 (낮을수록 높은 우선순위)
        /// </summary>
        public int GetStatePriority(PlayerState state)
        {
            var index = _priorityOrder.IndexOf(state);
            return index >= 0 ? index : int.MaxValue;
        }

        /// <summary>
        /// 두 상태 중 더 높은 우선순위를 가진 상태를 반환
        /// </summary>
        public PlayerState GetHigherPriorityState(PlayerState state1, PlayerState state2)
        {
            return GetStatePriority(state1) < GetStatePriority(state2) ? state1 : state2;
        }
    }
}