using Godot;
using projectgodot.Scripts.Interfaces;

namespace projectgodot
{
    public class PlayerMovement : IMovement
    {
        public Vector2 CalculateVelocity(Vector2 direction, float speed, float delta)
        {
            return direction * speed * delta;
        }
    }
}