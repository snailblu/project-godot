using Godot;

namespace projectgodot
{
    public class PlayerMovement
    {
        public Vector2 CalculateVelocity(Vector2 direction, float speed, float delta)
        {
            return direction * speed * delta;
        }
    }
}