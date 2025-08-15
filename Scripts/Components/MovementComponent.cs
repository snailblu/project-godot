using Godot;

namespace projectgodot
{
    /// <summary>
    /// Component that handles movement physics for CharacterBody2D entities.
    /// Separates movement logic from input handling for better testability and reusability.
    /// </summary>
    public partial class MovementComponent : Node2D, IMovable
    {
        [Export]
        public float Speed { get; set; } = 300.0f;

        public Vector2 CurrentDirection { get; private set; } = Vector2.Zero;
        public Vector2 LastDirection { get; private set; } = new Vector2(0, 1);
        public bool IsMoving => CurrentDirection != Vector2.Zero;

        private CharacterBody2D _character;

        public override void _Ready()
        {
            _character = GetParent<CharacterBody2D>();
            if (_character == null)
            {
                GD.PrintErr("MovementComponent must be a child of CharacterBody2D!");
                return;
            }
        }

        /// <summary>
        /// Process movement based on input intent
        /// </summary>
        public void ProcessMovement(InputIntent intent, double delta)
        {
            if (_character == null)
                return;

            CurrentDirection = intent.MovementDirection;

            if (intent.IsMoving)
            {
                _character.Velocity = CurrentDirection * Speed;
                LastDirection = CurrentDirection;
            }
            else
            {
                _character.Velocity = Vector2.Zero;
            }

            _character.MoveAndSlide();
        }
    }
}
