using Godot;
using projectgodot;

public enum PlayerState
{
    Moving, // 자유롭게 이동 가능한 기본 상태
    Attacking, // 공격 중이며, 이동이 제한되는 상태
}

/// <summary>
/// Refactored PlayerController using component-based architecture.
/// Separates input, movement, and interaction concerns for better testability and reusability.
/// </summary>
public partial class PlayerController : CharacterBody2D, IMovable
{
    public PlayerState CurrentState { get; private set; } = PlayerState.Moving;

    // IMovable implementation - delegated to MovementComponent
    public float Speed => _movementComponent?.Speed ?? 300.0f;
    public Vector2 CurrentDirection => _movementComponent?.CurrentDirection ?? Vector2.Zero;
    public Vector2 LastDirection => _movementComponent?.LastDirection ?? new Vector2(0, 1);
    public bool IsMoving => _movementComponent?.IsMoving ?? false;

    // Component references
    private IInputProvider _inputProvider;
    private MovementComponent _movementComponent;
    private InteractionComponent _interactionComponent;
    private PlayerAttackComponent _attackComponent;
    private HealthComponent _healthComponent;

    public override void _Ready()
    {
        // Initialize components
        _inputProvider = GetNode<KeyboardInputProvider>("KeyboardInputProvider");
        _movementComponent = GetNode<MovementComponent>("MovementComponent");
        _interactionComponent = GetNode<InteractionComponent>("InteractionComponent");
        _attackComponent = GetNode<PlayerAttackComponent>("PlayerAttackComponent");
        _healthComponent = GetNode<HealthComponent>("HealthComponent");

        // Validate required components
        if (_healthComponent == null)
        {
            GD.PrintErr("Player에 HealthComponent가 없습니다! 씬 구성을 확인하세요.");
            return;
        }

        if (_inputProvider == null)
        {
            GD.PrintErr("Player에 KeyboardInputProvider가 없습니다! 씬 구성을 확인하세요.");
            return;
        }

        if (_movementComponent == null)
        {
            GD.PrintErr("Player에 MovementComponent가 없습니다! 씬 구성을 확인하세요.");
            return;
        }

        // Connect health component events
        _healthComponent.Died += _OnPlayerDied;
        if (_attackComponent != null)
        {
            _attackComponent.AttackFinished += OnAttackFinished;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update input provider
        _inputProvider?.UpdateInput(delta);

        // Get input intent
        var intent = _inputProvider?.GetInputIntent() ?? InputIntent.None;

        // Process movement through component
        if (CurrentState == PlayerState.Moving)
        {
            _movementComponent?.ProcessMovement(intent, delta);
        }
        else
        {
            // 공격 중일 때는 속도를 0으로 고정하여 미끄러짐을 방지합니다.
            this.Velocity = Vector2.Zero;
        }

        // Handle actions
        if (intent.WantsToInteract && _interactionComponent != null)
        {
            _interactionComponent.TryInteract(this, LastDirection);
        }

        if (
            intent.WantsToAttack
            && CurrentState == PlayerState.Moving
            && _attackComponent != null
            && _attackComponent.CanAttack()
        )
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        // 1. 상태를 'Attacking'으로 변경하여 이동을 막습니다.
        CurrentState = PlayerState.Attacking;

        // 2. 공격을 수행합니다.
        _attackComponent.PerformAttack(LastDirection);

        // TODO: 애니메이션 컨트롤러에 공격 사실을 알리는 이벤트 발생
        // OnAttackPerformed?.Invoke();
    }

    private void OnAttackFinished()
    {
        // 3. 공격이 끝나면 다시 이동 가능한 상태로 돌아옵니다.
        CurrentState = PlayerState.Moving;
    }

    // IMovable implementation - delegate to MovementComponent
    public void ProcessMovement(InputIntent intent, double delta)
    {
        _movementComponent?.ProcessMovement(intent, delta);
    }

    private void _OnPlayerDied()
    {
        GD.Print("플레이어가 사망했습니다. 게임 오버 처리 시작!");
        Freeze();
    }

    /// <summary>
    /// Completely freezes the player - stops all movement, input, collisions, and components.
    /// Used for death handling to ensure no residual systems continue running.
    /// </summary>
    private void Freeze()
    {
        // Disable processing
        this.ProcessMode = ProcessModeEnum.Disabled;

        // Stop all movement
        this.Velocity = Vector2.Zero;

        // Disable collision detection
        this.SetCollisionLayerValue(1, false); // Player collision layer
        this.SetCollisionMaskValue(1, false); // Player collision mask

        // Disable input processing
        _inputProvider = null;

        // TODO: Stop all component activities
        // _movementComponent?.SetEnabled(false);
        // _attackComponent?.SetEnabled(false);
        // _interactionComponent?.SetEnabled(false);

        // TODO: Cancel any ongoing timers/cooldowns in components
        // _attackComponent?.CancelAllTimers();
    }

    public void ApplyDamage(float amount)
    {
        _healthComponent?.TakeDamage(amount);
    }

    /// <summary>
    /// Allows swapping input providers for AI control, testing, etc.
    /// </summary>
    public void SetInputProvider(IInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }
}
