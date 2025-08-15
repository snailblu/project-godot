// AnimationController.cs
using Godot;

public partial class AnimationController : Node
{
    #region 노드 경로 설정 (Node Path Configuration)

    [Export] public NodePath SpriteNodePath { get; set; } = "../AnimatedSprite2D";
    [Export] public NodePath StateMachinePath { get; set; } = "../StateMachine";

    #endregion

    private AnimatedSprite2D _sprite;
    private CharacterBody2D _owner;
    private StateMachine _stateMachine;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>(SpriteNodePath);
        _owner = GetParent<CharacterBody2D>();
        // 자기 자신의 AnimationFinished 시그널을 직접 처리
        _sprite.AnimationFinished += OnAnimationFinished;
        _stateMachine = GetNode<StateMachine>(StateMachinePath);
    }

    public override void _PhysicsProcess(double delta)
    {
        // 이동 방향에 따른 좌우 반전 로직
        if (Mathf.Abs(_owner.Velocity.X) > 0.1f)
        {
            _sprite.FlipH = _owner.Velocity.X < 0;
        }
    }

    // 외부(StateMachine)에서 호출할 공개 메서드
    public void PlayAnimation(string animName)
    {
        if (_sprite.Animation != animName)
        {
            _sprite.Play(animName);
        }
    }

    // 애니메이션 완료 시그널 처리 로직
    private void OnAnimationFinished()
    {
        string finishedAnim = _sprite.Animation;

        if (finishedAnim == "attack")
        {
            // 공격 애니메이션이 끝났다면,
            // StateMachine의 현재 상태가 여전히 "Attacking" (즉, 쿨타임 대기 중)인지 확인합니다.
            // 그렇다면, 대기 애니메이션을 재생합니다.
            if (_stateMachine.CurrentState == StateMachine.State.Attacking)
            {
                PlayAnimation("idle");
            }
        }
        else if (finishedAnim == "die")
        {
            _owner.QueueFree();
        }
    }
}
