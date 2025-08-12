using Godot;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 플레이어 애니메이션 컨트롤을 담당하는 인터페이스
    /// 이동 상태에 따른 애니메이션 처리
    /// </summary>
    public interface IPlayerAnimation
    {
        /// <summary>
        /// 애니메이션 시스템 초기화
        /// </summary>
        /// <param name="animationTree">Godot 애니메이션 트리</param>
        void Initialize(AnimationTree animationTree);
        
        /// <summary>
        /// 애니메이션 업데이트
        /// </summary>
        /// <param name="movementDirection">이동 방향 벡터</param>
        void UpdateAnimation(Vector2 movementDirection);
    }
}