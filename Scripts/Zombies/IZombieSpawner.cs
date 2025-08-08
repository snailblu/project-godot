using Godot;

namespace projectgodot
{
    /// <summary>
    /// 좀비 스포너 인터페이스 (테스트를 위한 추상화)
    /// </summary>
    public interface IZombieSpawner
    {
        /// <summary>
        /// 좀비를 생성합니다
        /// </summary>
        /// <param name="zombieScene">좀비 씬</param>
        /// <param name="position">생성 위치</param>
        /// <returns>생성된 좀비 인스턴스</returns>
        Zombie SpawnZombie(PackedScene zombieScene, Vector2 position);
    }
}