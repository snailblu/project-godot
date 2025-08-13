// PlayerController.cs
using Godot;

public partial class PlayerController : CharacterBody2D
{
    // 디자이너가 에디터에서 이동 속도를 쉽게 조절할 수 있도록 Export.
    [Export]
    public float Speed { get; set; } = 300.0f;
    private Area2D _interactionArea;
    private HealthComponent _healthComponent;
    private AnimationTree _animationTree;
    private Vector2 _lastDirection = new Vector2(0, 1);

    public override void _Ready()
    {
        _interactionArea = GetNode<Area2D>("InteractionArea");
        // 1. 자신의 자식 노드 중에서 "HealthComponent"라는 이름을 가진 노드를 찾아온다.
        _healthComponent = GetNode<HealthComponent>("HealthComponent");

        // 안전 장치: 만약 HealthComponent를 찾지 못했다면 에러를 출력한다.
        if (_healthComponent == null)
        {
            GD.PrintErr("Player에 HealthComponent가 없습니다! 씬 구성을 확인하세요.");
            return;
        }

        // 2. HealthComponent의 'Died' 시그널(방송)이 발생하면,
        //    이 스크립트의 '_OnPlayerDied' 함수를 실행하도록 '구독' 신청한다.
        _healthComponent.Died += _OnPlayerDied;

        _animationTree = GetNode<AnimationTree>("AnimationTree");
        // AnimationTree를 활성화해야 작동합니다.
        _animationTree.Active = true;
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        if (direction != Vector2.Zero)
        {
            _lastDirection = direction;
        }

        Velocity = direction * Speed;

        MoveAndSlide();

        // 애니메이션 파라미터 업데이트
        UpdateAnimationParameters(direction);

    }
    public override void _Input(InputEvent @event)
    {
        // "interact" 행동(예: E 키)이 눌렸을 때
        if (@event.IsActionPressed("interact"))
        {
            // 1. 상호작용 영역 안에 있는 개체들을 가져온다.
            var overlappingBodies = _interactionArea.GetOverlappingBodies();
            foreach (var body in overlappingBodies)
            {
                // 2. 개체가 'IInteractable' 계약을 따르는지 확인한다.
                if (body is IInteractable interactable)
                {
                    // 3. 따르는 경우, 상호작용을 실행시키고 반복을 멈춘다 (가장 가까운 것 하나만).
                    interactable.Interact(this);
                    break;
                }
            }
        }
    }
    private void _OnPlayerDied()
    {
        // '사망' 방송을 수신했을 때 실행할 로직
        GD.Print("플레이어가 사망했습니다. 게임 오버 처리 시작!");

        // 움직임을 멈춘다.
        this.ProcessMode = ProcessModeEnum.Disabled;

        // 여기에 게임 오버 UI를 띄우거나, 사망 애니메이션을 재생하는 코드를 추가.
        // 예시: GetTree().ChangeSceneToFile("res://game_over_screen.tscn");
    }

    // 예시: 플레이어가 데미지를 입는 함수
    public void ApplyDamage(float amount)
    {
        _healthComponent.TakeDamage(amount);
    }

    private void UpdateAnimationParameters(Vector2 moveDirection)
    {
        bool isMoving = moveDirection != Vector2.Zero;

        // 1. 상태 머신 전환 조건 설정
        _animationTree.Set("parameters/StateMachine/MoveState/conditions/Run", isMoving);
        _animationTree.Set("parameters/StateMachine/MoveState/conditions/Idle", !isMoving);

        Vector2 animationDirection = moveDirection;

        // 2. BlendSpace2D의 블렌드 위치 설정
        if (!isMoving)
        {
            animationDirection = _lastDirection;
        }
        // 핵심 수정: AnimationTree가 사용하는 좌표계에 맞게 Y축을 뒤집어 줍니다.
        animationDirection.Y = -animationDirection.Y;
        
        // 이제 뒤집힌 벡터를 사용합니다.
        _animationTree.Set("parameters/StateMachine/MoveState/RunState/blend_position", animationDirection);
        _animationTree.Set("parameters/StateMachineMoveState/IdleState/blend_position", animationDirection);

    }

}