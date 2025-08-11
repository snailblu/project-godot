using System;
using System.Collections.Generic;
using Godot;
using projectgodot.Components.PlayerStates;
using projectgodot.Utils;

namespace projectgodot.Components
{
    /// <summary>
    /// 플레이어의 상태를 관리하는 State Machine 컴포넌트
    /// 컴포지션 패턴을 유지하면서 상태 기반 로직을 중앙화
    /// </summary>
    public partial class PlayerStateMachine : Node
    {
        private Dictionary<PlayerState, IPlayerState> _states;
        private IPlayerState _currentState;
        private PlayerState _currentStateName;
        
        // Player 참조 (상태들이 플레이어 정보에 접근하기 위해)
        public Player Player { get; private set; }
        
        // 입력 상태 추적 (상태 전환 결정에 사용)
        public bool HasMovementInput { get; private set; }
        
        // 상태 전환 이벤트
        public event Action<PlayerState, PlayerState> StateChanged;
        
        public PlayerState CurrentState => _currentStateName;
        
        public override void _Ready()
        {
            // Player 참조 획득
            Player = GetParent() as Player;
            if (Player == null)
            {
                GodotLogger.SafePrint("ERROR: PlayerStateMachine must be a child of Player");
                return;
            }
            
            // 모든 상태 초기화
            InitializeStates();
            
            // 초기 상태를 Idle로 설정
            TransitionToState(PlayerState.Idle, "Initial state");
        }
        
        protected virtual void InitializeStates()
        {
            _states = new Dictionary<PlayerState, IPlayerState>
            {
                { PlayerState.Idle, new IdleState(this) },
                { PlayerState.Moving, new MovingState(this) },
                { PlayerState.Dashing, new DashingState(this) },
                { PlayerState.Shooting, new ShootingState(this) },
                { PlayerState.TakingDamage, new TakingDamageState(this) },
                { PlayerState.Starving, new StarvingState(this) },
                { PlayerState.Dead, new DeadState(this) }
            };
        }
        
        public override void _Process(double delta)
        {
            // 현재 상태 업데이트
            _currentState?.OnUpdate(delta);
            
            // 자동 상태 전환 체크
            CheckAutoStateTransitions();
        }
        
        /// <summary>
        /// 외부에서 상태 전환을 요청
        /// </summary>
        public bool RequestStateTransition(PlayerState targetState, string reason = "", object data = null)
        {
            if (_currentStateName == targetState)
            {
                return false; // 이미 같은 상태
            }
            
            var targetStateImpl = _states[targetState];
            if (!targetStateImpl.CanTransitionFrom(_currentStateName, data))
            {
                GodotLogger.SafePrint($"State transition denied: {_currentStateName} -> {targetState} (Reason: {reason})");
                return false;
            }
            
            return TransitionToState(targetState, reason, data);
        }
        
        protected bool TransitionToState(PlayerState targetState, string reason, object data = null)
        {
            var previousState = _currentStateName;
            var targetStateImpl = _states[targetState];
            
            // 현재 상태 종료
            _currentState?.OnExit(targetState);
            
            // 새 상태로 전환
            _currentState = targetStateImpl;
            _currentStateName = targetState;
            
            // 새 상태 진입
            _currentState.OnEnter(previousState, data);
            
            // 이벤트 발생
            StateChanged?.Invoke(previousState, targetState);
            
            GodotLogger.SafePrint($"State transition: {previousState} -> {targetState} (Reason: {reason})");
            return true;
        }
        
        /// <summary>
        /// 자동 상태 전환을 체크 (우선순위 기반)
        /// </summary>
        private void CheckAutoStateTransitions()
        {
            if (Player == null) return;
            
            // 최고 우선순위: Dead
            if (Player.Health.IsDead && _currentStateName != PlayerState.Dead)
            {
                RequestStateTransition(PlayerState.Dead, "Player health reached zero");
                return;
            }
            
            // Dead 상태면 더 이상 체크하지 않음
            if (_currentStateName == PlayerState.Dead) return;
            
            // 높은 우선순위: Starving (단, TakingDamage나 Dashing이 아닐 때)
            if (Player._hungerComponent.IsStarving && 
                _currentStateName != PlayerState.Starving &&
                _currentStateName != PlayerState.TakingDamage &&
                _currentStateName != PlayerState.Dashing)
            {
                RequestStateTransition(PlayerState.Starving, "Player is starving");
                return;
            }
            
            // Dashing 상태 종료 체크
            if (_currentStateName == PlayerState.Dashing && !Player._dash.IsDashing)
            {
                var targetState = HasMovementInput ? PlayerState.Moving : PlayerState.Idle;
                RequestStateTransition(targetState, "Dashing completed");
                return;
            }
            
            // Moving/Idle 상태 간 전환 (낮은 우선순위)
            if (_currentStateName == PlayerState.Idle && HasMovementInput)
            {
                RequestStateTransition(PlayerState.Moving, "Movement input detected");
            }
            else if (_currentStateName == PlayerState.Moving && !HasMovementInput)
            {
                RequestStateTransition(PlayerState.Idle, "No movement input");
            }
        }
        
        /// <summary>
        /// 입력 핸들러에서 호출: 이동 입력 상태 업데이트
        /// </summary>
        public void UpdateMovementInput(bool hasMovement)
        {
            HasMovementInput = hasMovement;
        }
        
        /// <summary>
        /// 대시 요청 처리
        /// </summary>
        public bool RequestDash()
        {
            return RequestStateTransition(PlayerState.Dashing, "Dash requested");
        }
        
        /// <summary>
        /// 발사 요청 처리
        /// </summary>
        public bool RequestShoot()
        {
            return RequestStateTransition(PlayerState.Shooting, "Shoot requested");
        }
        
        /// <summary>
        /// 데미지 요청 처리
        /// </summary>
        public bool RequestTakeDamage()
        {
            return RequestStateTransition(PlayerState.TakingDamage, "Damage received");
        }
        
        /// <summary>
        /// 현재 상태가 특정 상태인지 확인
        /// </summary>
        public bool IsInState(PlayerState state)
        {
            return _currentStateName == state;
        }
        
        /// <summary>
        /// 현재 상태에서 특정 행동이 가능한지 확인
        /// </summary>
        public bool CanPerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "move":
                    return _currentStateName != PlayerState.Dead && 
                           _currentStateName != PlayerState.TakingDamage;
                           
                case "dash":
                    return _currentStateName != PlayerState.Dead && 
                           _currentStateName != PlayerState.TakingDamage &&
                           _currentStateName != PlayerState.Dashing;
                           
                case "shoot":
                    return _currentStateName != PlayerState.Dead && 
                           _currentStateName != PlayerState.TakingDamage &&
                           _currentStateName != PlayerState.Dashing;
                           
                case "takedamage":
                    return _currentStateName != PlayerState.Dead;
                    
                default:
                    return false;
            }
        }
    }
}