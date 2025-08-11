using Godot;

namespace projectgodot
{
    public partial class SoundManager : Node
    {
        private AudioStreamPlayer weaponFirePlayer;
        private AudioStreamPlayer zombieDamagePlayer;
        private AudioStreamPlayer playerDamagePlayer;
        private AudioStreamPlayer projectileHitPlayer;

        public override void _Ready()
        {
            // AudioStreamPlayer 노드들 생성
            weaponFirePlayer = new AudioStreamPlayer();
            zombieDamagePlayer = new AudioStreamPlayer();
            playerDamagePlayer = new AudioStreamPlayer();
            projectileHitPlayer = new AudioStreamPlayer();

            // 자식 노드로 추가
            AddChild(weaponFirePlayer);
            AddChild(zombieDamagePlayer);
            AddChild(playerDamagePlayer);
            AddChild(projectileHitPlayer);

            // 오디오 스트림 로드
            weaponFirePlayer.Stream = GD.Load<AudioStream>("res://Audio/Effects/fire.wav");
            zombieDamagePlayer.Stream = GD.Load<AudioStream>("res://Audio/Effects/zombiehit.wav");
            playerDamagePlayer.Stream = GD.Load<AudioStream>("res://Audio/Effects/hit.wav");
            projectileHitPlayer.Stream = GD.Load<AudioStream>("res://Audio/Effects/hit.wav");

            // Events 싱글톤의 시그널들을 구독
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
            {
                events.PlayerFiredWeapon += OnPlayerFiredWeapon;
                events.ZombieTookDamage += OnZombieTookDamage;
                events.PlayerTookDamage += OnPlayerTookDamage;
                events.ProjectileHitWall += OnProjectileHitWall;
            }

            GD.Print("SoundManager가 초기화되었습니다.");
        }

        private void OnPlayerFiredWeapon()
        {
            weaponFirePlayer.Play();
        }

        private void OnZombieTookDamage()
        {
            zombieDamagePlayer.Play();
        }

        private void OnPlayerTookDamage()
        {
            playerDamagePlayer.Play();
        }

        private void OnProjectileHitWall()
        {
            GD.Print("총알 벽 충돌 사운드 재생");
            projectileHitPlayer.Play();
        }

        public override void _ExitTree()
        {
            // 시그널 연결 해제
            var events = EventsHelper.GetEventsNodeSafe(this);
            if (events != null)
            {
                events.PlayerFiredWeapon -= OnPlayerFiredWeapon;
                events.ZombieTookDamage -= OnZombieTookDamage;
                events.PlayerTookDamage -= OnPlayerTookDamage;
                events.ProjectileHitWall -= OnProjectileHitWall;
            }
        }
    }
}