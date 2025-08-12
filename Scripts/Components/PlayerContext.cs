using Godot;
using projectgodot.Scripts.Interfaces;

namespace projectgodot.Components
{
    /// <summary>
    /// 컴포넌트들이 Player와 관련된 데이터에 접근할 수 있도록 하는 읽기전용 컨텍스트
    /// </summary>
    public sealed class PlayerContext
    {
        /// <summary>
        /// Player 인스턴스에 대한 읽기전용 참조
        /// </summary>
        public Player Player { get; }
        
        /// <summary>
        /// SceneFactory 인스턴스에 대한 읽기전용 참조
        /// </summary>
        public ISceneFactory SceneFactory { get; }
        
        /// <summary>
        /// Player의 글로벌 위치 (자주 사용되는 값을 캐싱)
        /// </summary>
        public Vector2 PlayerPosition => Player?.GlobalPosition ?? Vector2.Zero;
        
        /// <summary>
        /// PlayerContext 생성자
        /// </summary>
        /// <param name="player">Player 인스턴스</param>
        /// <param name="sceneFactory">SceneFactory 인스턴스</param>
        public PlayerContext(Player player, ISceneFactory sceneFactory)
        {
            Player = player ?? throw new System.ArgumentNullException(nameof(player));
            SceneFactory = sceneFactory; // null일 수 있음
        }
    }
}