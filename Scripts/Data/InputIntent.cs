using Godot;

namespace projectgodot
{
    /// <summary>
    /// Represents player input intentions separated from the input source.
    /// This allows different input providers (keyboard, AI, etc.) to produce
    /// the same data structure for movement and action processing.
    /// </summary>
    public struct InputIntent
    {
        /// <summary>
        /// Normalized movement direction vector (-1 to 1 on each axis)
        /// </summary>
        public Vector2 MovementDirection { get; set; }

        /// <summary>
        /// Whether the player intends to interact this frame
        /// </summary>
        public bool WantsToInteract { get; set; }

        /// <summary>
        /// Whether the player intends to attack this frame
        /// </summary>
        public bool WantsToAttack { get; set; }

        /// <summary>
        /// Whether the player is actively providing movement input
        /// </summary>
        public readonly bool IsMoving => MovementDirection != Vector2.Zero;

        public InputIntent(
            Vector2 movementDirection = default,
            bool wantsToInteract = false,
            bool wantsToAttack = false
        )
        {
            MovementDirection = movementDirection;
            WantsToInteract = wantsToInteract;
            WantsToAttack = wantsToAttack;
        }

        /// <summary>
        /// Creates an empty InputIntent with no actions
        /// </summary>
        public static InputIntent None => new InputIntent();

        /// <summary>
        /// Creates an InputIntent with only movement
        /// </summary>
        public static InputIntent MovementOnly(Vector2 direction) => new InputIntent(direction);
    }
}
