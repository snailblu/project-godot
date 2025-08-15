using Godot;

namespace projectgodot
{
    public interface IMovable
    {
        float Speed { get; }

        /// <summary>
        /// Apply movement based on input intent
        /// </summary>
        /// <param name="intent">Input intentions for this frame</param>
        /// <param name="delta">Time since last frame</param>
        void ProcessMovement(InputIntent intent, double delta);

        /// <summary>
        /// Get the current movement direction (normalized)
        /// </summary>
        Vector2 CurrentDirection { get; }

        /// <summary>
        /// Get the last non-zero movement direction (for attacks, etc.)
        /// </summary>
        Vector2 LastDirection { get; }

        /// <summary>
        /// Whether the entity is currently moving
        /// </summary>
        bool IsMoving { get; }
    }
}
