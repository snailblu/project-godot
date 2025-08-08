using Godot;

namespace projectgodot
{
    /// <summary>
    /// 게임 오브젝트 생성을 담당하는 팩토리 인터페이스
    /// </summary>
    public interface ISceneFactory
    {
        /// <summary>
        /// 좀비를 생성합니다
        /// </summary>
        /// <param name="scene">좀비 씬</param>
        /// <param name="position">생성 위치</param>
        /// <returns>생성된 좀비 인스턴스</returns>
        Zombie CreateZombie(PackedScene scene, Vector2 position);
        
        /// <summary>
        /// 발사체를 생성합니다
        /// </summary>
        /// <param name="scene">발사체 씬</param>
        /// <param name="position">생성 위치</param>
        /// <param name="direction">발사 방향</param>
        /// <param name="damage">발사체 데미지</param>
        /// <returns>생성된 발사체 인스턴스</returns>
        Projectile CreateProjectile(PackedScene scene, Vector2 position, Vector2 direction, int damage);
    }
}