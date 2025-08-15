using Godot;
using projectgodot;

/// <summary>
/// Refactored PlayerController using component-based architecture.
/// Separates input, movement, and interaction concerns for better testability and reusability.
/// </summary>
public partial class PlayerController : CharacterBody2D, IMovable
{
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
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update input provider
        _inputProvider?.UpdateInput(delta);

        // Get input intent
        var intent = _inputProvider?.GetInputIntent() ?? InputIntent.None;

        // Process movement through component
        _movementComponent?.ProcessMovement(intent, delta);

        // Handle actions
        if (intent.WantsToInteract && _interactionComponent != null)
        {
            _interactionComponent.TryInteract(this, LastDirection);
        }

        if (intent.WantsToAttack && _attackComponent != null && _attackComponent.CanAttack())
        {
            _attackComponent.PerformAttack(LastDirection);
        }
    }

    // IMovable implementation - delegate to MovementComponent
    public void ProcessMovement(InputIntent intent, double delta)
    {
        _movementComponent?.ProcessMovement(intent, delta);
    }

    private void _OnPlayerDied()
    {
        GD.Print("플레이어가 사망했습니다. 게임 오버 처리 시작!");
        this.ProcessMode = ProcessModeEnum.Disabled;
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
