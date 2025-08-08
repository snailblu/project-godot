using Godot;

namespace projectgodot
{
    public class ZombieAIComponent
    {
        public Vector2 CalculateDirection(Vector2 selfPosition, Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - selfPosition;
            
            // 길이가 0이면 제로 벡터 반환 (같은 위치인 경우)
            if (direction.LengthSquared() == 0)
            {
                return Vector2.Zero;
            }
            
            return direction.Normalized();
        }
    }
}