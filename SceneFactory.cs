using Godot;
using System;

namespace projectgodot
{
    /// <summary>
    /// 게임 오브젝트 생성을 담당하는 팩토리 클래스
    /// </summary>
    public partial class SceneFactory : Node, ISceneFactory
    {
        public event Action ZombieDied;

        public Zombie CreateZombie(PackedScene scene, Vector2 position)
        {
            var zombie = scene.Instantiate<Zombie>();
            zombie.GlobalPosition = position;
            
            // 현재 씬의 루트에 추가
            GetTree().CurrentScene.AddChild(zombie);
            
            // 한 프레임 후에 이벤트 연결
            CallDeferred(nameof(ConnectZombieEvents), zombie);
            
            return zombie;
        }

        public Projectile CreateProjectile(PackedScene scene, Vector2 position, Vector2 direction)
        {
            var projectile = scene.Instantiate<Projectile>();
            projectile.GlobalPosition = position;
            projectile.Direction = direction;
            
            // 현재 씬의 루트에 추가
            GetTree().CurrentScene.AddChild(projectile);
            
            return projectile;
        }

        /// <summary>
        /// 좀비 이벤트를 연결합니다 (지연 실행)
        /// </summary>
        private void ConnectZombieEvents(Zombie zombie)
        {
            if (zombie?.Health != null)
            {
                zombie.Health.Died += () => ZombieDied?.Invoke();
            }
            else
            {
                GD.PrintErr("좀비의 Health 컴포넌트가 초기화되지 않았습니다!");
            }
        }
    }
}