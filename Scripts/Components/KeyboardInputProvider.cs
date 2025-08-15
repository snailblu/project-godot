using Godot;

namespace projectgodot
{
    /// <summary>
    /// Input provider that reads keyboard and gamepad input using Godot's Input singleton.
    /// Implements the standard player control scheme for the game.
    /// </summary>
    public partial class KeyboardInputProvider : Node, IInputProvider
    {
        private InputIntent _currentIntent;

        public InputIntent GetInputIntent()
        {
            return _currentIntent;
        }

        public void UpdateInput(double delta)
        {
            // Read movement input
            Vector2 movementDirection = Input.GetVector(
                "move_left",
                "move_right",
                "move_up",
                "move_down"
            );

            // Read action inputs (only true on the frame they're pressed)
            bool wantsToInteract = Input.IsActionJustPressed("interact");
            bool wantsToAttack = Input.IsActionJustPressed("attack");

            _currentIntent = new InputIntent(
                movementDirection: movementDirection,
                wantsToInteract: wantsToInteract,
                wantsToAttack: wantsToAttack
            );
        }
    }
}
