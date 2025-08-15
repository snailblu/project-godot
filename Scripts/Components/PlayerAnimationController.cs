// PlayerAnimationController.cs
using Godot;

public partial class PlayerAnimationController : Node
{
    // 의존하는 다른 컴포넌트들
    private PlayerController _playerController;
    private PlayerAttackComponent _playerAttackComponent;
    private AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _playerController = GetParent<PlayerController>();
        _playerAttackComponent = GetParent().GetNode<PlayerAttackComponent>("PlayerAttackComponent");
        _animationTree = GetParent().GetNode<AnimationTree>("AnimationTree");
        _stateMachine = (AnimationNodeStateMachinePlayback)
            _animationTree.Get("parameters/StateMachine/playback");

        // PlayerAttackComponent의 공격 이벤트를 직접 '구독'합니다.
        _playerAttackComponent.OnAttackPerformed += PlayAttackAnimation;
    }

    public override void _Process(double delta)
    {
        // PlayerController의 상태를 '읽어서' AnimationTree 파라미터를 업데이트합니다.
        UpdateMoveAnimation();
    }

    private void UpdateMoveAnimation()
    {
        _animationTree.Set(
            "parameters/StateMachine/MoveState/conditions/Run",
            _playerController.IsMoving
        );
        _animationTree.Set(
            "parameters/StateMachine/MoveState/conditions/Idle",
            !_playerController.IsMoving
        );

        // BlendSpace 파라미터는 항상 마지막으로 바라본 방향을 사용합니다.
        Vector2 rawDirection = _playerController.LastDirection;
        Vector2 animationDirection = new Vector2(rawDirection.X, -rawDirection.Y);

        _animationTree.Set(
            "parameters/StateMachine/MoveState/IdleState/blend_position",
            animationDirection
        );
        _animationTree.Set(
            "parameters/StateMachine/MoveState/RunState/blend_position",
            animationDirection
        );
        _animationTree.Set(
            "parameters/StateMachine/AttackState/blend_position",
            animationDirection
        );
    }

    public override void _ExitTree()
    {
        // 이벤트 구독 해제로 메모리 누수 방지
        if (_playerAttackComponent != null)
        {
            _playerAttackComponent.OnAttackPerformed -= PlayAttackAnimation;
        }
    }

    private void PlayAttackAnimation()
    {
        // 공격 요청을 받으면 StateMachine의 상태를 전환합니다.
        _stateMachine.Travel("AttackState");
    }
}
