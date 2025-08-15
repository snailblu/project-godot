// Zombie.cs
using Godot;
using projectgodot;

[GlobalClass]
public partial class Zombie : CharacterBody2D, IMovable
{
    #region 노드 경로 설정 (Node Path Configuration)

    [Export]
    public NodePath HealthComponentPath { get; set; } = "HealthComponent";

    [Export]
    public NodePath AttackComponentPath { get; set; } = "AttackComponent";

    [Export]
    public NodePath StateMachinePath { get; set; } = "StateMachine";

    [Export]
    public NodePath AnimationControllerPath { get; set; } = "AnimationController";

    [Export]
    public NodePath DetectionAreaPath { get; set; } = "DetectionArea";

    #endregion

    #region 컴포넌트 참조 (Component References)

    // 이 좀비가 의존하는 핵심 부품들입니다.
    private HealthComponent _healthComponent;
    private ZombieAttackComponent _attackComponent;
    private StateMachine _stateMachine;
    private AnimationController _animationController;
    private Area2D _detectionArea;

    #endregion

    #region 능력치 (Stats)

    /// <summary>
    /// 좀비의 이동 속도입니다.
    /// 디자이너가 에디터에서 쉽게 조절할 수 있도록 [Export]합니다.
    /// StateMachine은 이 값을 Get("Speed")를 통해 참조합니다.
    /// </summary>
    [Export]
    public float Speed { get; private set; } = 100.0f;

    #endregion

    #region IMovable Implementation

    // IMovable properties - Zombies delegate movement to StateMachine
    public Vector2 CurrentDirection => _stateMachine?.CurrentDirection ?? Vector2.Zero;
    public Vector2 LastDirection => _stateMachine?.LastDirection ?? Vector2.Zero;
    public bool IsMoving => _stateMachine?.IsMoving ?? false;

    /// <summary>
    /// Zombies don't use InputIntent directly - movement is controlled by StateMachine
    /// This method is provided for IMovable compliance but doesn't affect zombie behavior
    /// </summary>
    public void ProcessMovement(InputIntent intent, double delta)
    {
        // Zombies are controlled by AI through StateMachine, not input
        // This method exists for IMovable interface compliance
    }

    #endregion

    #region Godot 생명주기 메서드 (Godot Lifecycle Methods)

    public override void _Ready()
    {
        // 1. 필요한 모든 부품(컴포넌트)을 찾아와 변수에 할당합니다.
        _healthComponent = GetNode<HealthComponent>(HealthComponentPath);
        _attackComponent = GetNode<ZombieAttackComponent>(AttackComponentPath);
        _stateMachine = GetNode<StateMachine>(StateMachinePath);
        _animationController = GetNode<AnimationController>(AnimationControllerPath);
        _detectionArea = GetNode<Area2D>(DetectionAreaPath);

        _healthComponent.Died += OnDied;
        _detectionArea.BodyEntered += OnDetectionAreaEntered;
        _detectionArea.BodyExited += OnDetectionAreaExited;
        _stateMachine.OnStateChanged += HandleStateChange;
        _stateMachine.OnAttackRequested += HandleAttackRequest;
        _attackComponent.AttackEnded += HandleAttackEnded;
    }

    #endregion

    #region 이벤트 핸들러 (Event Handlers)

    private void OnDied()
    {
        // StateMachine에게 사망 상태로 전환 요청
        // 나머지 정리 작업은 StateMachine.EnterState(Dying)에서 일괄 처리
        _stateMachine.CurrentState = StateMachine.State.Dying;
    }

    private void OnDetectionAreaEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            _stateMachine.Target = body;

            if (_stateMachine.CurrentState == StateMachine.State.Idle)
            {
                _stateMachine.CurrentState = StateMachine.State.Chasing;
            }
        }
    }

    private void OnDetectionAreaExited(Node2D body)
    {
        if (body == _stateMachine.Target)
        {
            // StateMachine에게 목표물을 잃어버렸다고 알려줍니다.
            _stateMachine.Target = null;
        }
    }

    private void HandleStateChange(StateMachine.State newState)
    {
        string animationName = "";
        switch (newState)
        {
            case StateMachine.State.Idle:
                animationName = "idle";
                break;
            case StateMachine.State.Chasing:
                animationName = "walk"; // "chasing" 대신 "walk"를 사용할 수 있습니다.
                break;
            case StateMachine.State.Attacking:
                animationName = "attack"; // "attacking" 대신 "attack"을 사용합니다.
                break;
            case StateMachine.State.Dying:
                animationName = "die"; // "dying" 대신 "die"를 사용합니다.
                break;
        }

        if (animationName != "")
        {
            _animationController.PlayAnimation(animationName);
        }
    }

    private void HandleAttackRequest(Node2D target)
    {
        _attackComponent.PerformAttack(target);
    }

    private void HandleAttackEnded()
    {
        // 공격이 완료되었으므로 Chasing 상태로 돌아갑니다.
        if (_stateMachine.CurrentState == StateMachine.State.Attacking)
        {
            _stateMachine.CurrentState = StateMachine.State.Chasing;
        }
    }

    #endregion
}
