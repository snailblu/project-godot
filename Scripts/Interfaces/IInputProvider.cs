using Godot;

namespace projectgodot
{
    /// <summary>
    /// Interface for different input sources that can provide player intentions.
    /// Implementations can include keyboard input, AI controllers, replay systems, etc.
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Gets the current input intentions for this frame
        /// </summary>
        /// <returns>InputIntent containing movement direction and action flags</returns>
        InputIntent GetInputIntent();

        /// <summary>
        /// Called each frame to update input state (e.g., read from Input singleton)
        /// </summary>
        /// <param name="delta">Time since last frame</param>
        void UpdateInput(double delta);
    }
}
