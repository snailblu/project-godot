using projectgodot.Components;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 모든 게임 컴포넌트가 구현해야 하는 기본 인터페이스
    /// </summary>
    public interface IGameComponent
    {
        /// <summary>
        /// 컴포넌트 초기화 메서드
        /// PlayerContext를 통해 필요한 의존성에 접근할 수 있음
        /// </summary>
        /// <param name="context">Player와 관련된 컨텍스트 정보</param>
        void Initialize(PlayerContext context);
    }
}