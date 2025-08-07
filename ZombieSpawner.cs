using Godot;

namespace projectgodot
{
    /// <summary>
    /// 좀비 생성을 담당하는 스포너 클래스
    /// </summary>
    public class ZombieSpawner : IZombieSpawner
    {
        private readonly ISceneFactory _sceneFactory;

        public ZombieSpawner(ISceneFactory sceneFactory)
        {
            _sceneFactory = sceneFactory;
        }

        /// <summary>
        /// 좀비를 생성합니다
        /// </summary>
        /// <param name="zombieScene">좀비 씬</param>
        /// <param name="position">생성 위치</param>
        /// <returns>생성된 좀비 인스턴스, 실패시 null</returns>
        public Zombie SpawnZombie(PackedScene zombieScene, Vector2 position)
        {
            if (zombieScene == null)
            {
                return null;
            }

            return _sceneFactory.CreateZombie(zombieScene, position);
        }
    }
}