// StateMachine.cs
using System;
using Godot;
using projectgodot;

public partial class StateMachine : Node
{
    #region 상태 정의 (State Definition)

    public enum State
    {
        Idle,
        Chasing,
        Attacking,
        Dying,
    }

    private State _currentState = State.Idle;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                ExitState(_currentState);
                _currentState = value;
                EnterState(_currentState);
            }
        }
    }

    #endregion

    #region 노드 경로 설정 (Node Path Configuration)

    [Export] public NodePath AttackComponentPath { get; set; } = "AttackComponent";
    [Export] public NodePath CollisionShapePath { get; set; } = "CollisionShape2D";

    #endregion

    #region 이벤트 및 속성

    public event Action<State> OnStateChanged;
    public event Action<Node2D> OnAttackRequested;
    public Node2D Target { get; set; } = null;
    private CharacterBody2D _owner;
    private ZombieAttackComponent _attackComponent;

    // Movement tracking for IMovable compatibility
    public Vector2 CurrentDirection { get; private set; } = Vector2.Zero;
    public Vector2 LastDirection { get; private set; } = Vector2.Zero;
    public bool IsMoving => CurrentDirection != Vector2.Zero;

    #endregion

    #region Godot 생명주기 메서드

    public override void _Ready()
    {
        _owner = GetParent<CharacterBody2D>();
        if (_owner != null)
        {
            _attackComponent = _owner.GetNode<ZombieAttackComponent>(AttackComponentPath);
        }
        else
        {
            GD.PrintErr("StateMachine은 CharacterBody2D의 자식이어야 합니다!");
            SetProcess(false);
        }
        EnterState(CurrentState);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState == State.Dying || _owner == null)
            return;
        
        // Target 유효성 검증 - 타깃이 파괴되었거나 무효하면 Idle로 전환
        if (Target != null && !GodotObject.IsInstanceValid(Target))
        {
            Target = null;
        }
        
        if (Target == null && CurrentState != State.Idle)
            CurrentState = State.Idle;

        // 각 상태의 '업데이트' 로직만 호출합니다.
        switch (CurrentState)
        {
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Chasing:
                UpdateChasingState();
                break;
            case State.Attacking:
                UpdateAttackingState();
                break;
        }
    }

    #endregion

    #region 상태 진입/퇴장/업데이트 로직

    private void EnterState(State newState)
    {
        // OnStateChanged 이벤트는 상태가 실제로 변경되었음을 외부에 알리는 역할만 합니다.
        OnStateChanged?.Invoke(newState);

        // 상태에 처음 진입할 때 딱 한 번 실행되는 로직
        switch (newState)
        {
            case State.Attacking:
                _owner.Velocity = Vector2.Zero;
                OnAttackRequested?.Invoke(Target); // 공격 요청!
                break;
            case State.Dying:
                // 사망 상태 진입 시 모든 물리 처리 정지 및 정리
                _owner.Velocity = Vector2.Zero;
                _owner.SetPhysicsProcess(false);
                _owner.GetNode<CollisionShape2D>(CollisionShapePath).Disabled = true;
                SetPhysicsProcess(false); // StateMachine 자체도 정지
                break;
        }
    }

    private void ExitState(State oldState)
    {
        // 특정 상태를 떠날 때 정리해야 할 로직이 있다면 여기에 추가
    }

    // --- 함수의 이름들을 Update...로 변경하여 역할을 명확히 함 ---
    private void UpdateIdleState()
    {
        _owner.Velocity = Vector2.Zero;
        CurrentDirection = Vector2.Zero;
        _owner.MoveAndSlide();
    }

    private void UpdateChasingState()
    {
        if (Target == null)
            return;

        // --- 여기가 핵심적인 변경점 2 ---
        // 이제 Chasing 상태는 쿨타임을 전혀 신경쓰지 않습니다.
        // 오직 거리만 보고 Attacking 상태로 전환할지, 계속 추적할지만 결정합니다.
        float distanceToTarget = _owner.GlobalPosition.DistanceTo(Target.GlobalPosition);

        if (distanceToTarget <= _attackComponent.AttackRange)
        {
            // 공격이 가능한지만 확인하고, 가능하면 즉시 공격 상태로 전환
            if (_attackComponent.CanAttack())
            {
                CurrentState = State.Attacking;
            }
            else
            {
                // 공격 쿨타임 중이라면, 공격 범위 안에서 멈춰 서서 기다립니다. (선택적)
                _owner.Velocity = Vector2.Zero;
                CurrentDirection = Vector2.Zero;
            }
        }
        else
        {
            Vector2 direction = (Target.GlobalPosition - _owner.GlobalPosition).Normalized();
            float speed = (_owner as IMovable)?.Speed ?? 0f;
            _owner.Velocity = direction * speed;
            CurrentDirection = direction;
            if (direction != Vector2.Zero)
                LastDirection = direction;
        }
        _owner.MoveAndSlide();
    }

    // Attacking 상태는 이제 AttackComponent의 이벤트에 의해서만 종료됩니다.
    private void UpdateAttackingState()
    {
        // Target이 공격 범위를 크게 벗어나면 즉시 Chasing으로 복귀
        if (Target != null)
        {
            float distanceToTarget = _owner.GlobalPosition.DistanceTo(Target.GlobalPosition);
            float cancelThreshold = _attackComponent.AttackRange * 1.2f;
            
            if (distanceToTarget > cancelThreshold)
            {
                CurrentState = State.Chasing;
                return;
            }
        }
        
        // Attacking 상태에서는 움직이지 않도록 합니다.
        _owner.Velocity = Vector2.Zero;
        CurrentDirection = Vector2.Zero;
        _owner.MoveAndSlide();
        
        // 상태 전환은 AttackComponent의 AttackEnded 이벤트에서 처리됩니다.
    }
    #endregion
}
