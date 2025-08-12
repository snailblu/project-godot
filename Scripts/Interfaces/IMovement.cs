using Godot;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 이동 로직을 담당하는 인터페이스
    /// 캐릭터의 이동 계산을 처리
    /// </summary>
    public interface IMovement
    {
        /// <summary>
        /// 입력 방향과 속도를 기반으로 실제 속도 벡터를 계산
        /// </summary>
        /// <param name="direction">이동 방향</param>
        /// <param name="speed">이동 속도</param>
        /// <param name="delta">프레임 델타 시간</param>
        /// <returns>계산된 속도 벡터</returns>
        Vector2 CalculateVelocity(Vector2 direction, float speed, float delta);
    }
}