using Godot;

namespace projectgodot
{
    // Godot AutoLoad로 등록될 전역 싱글톤 이벤트 버스
    public partial class Events : Node
    {
        // 좀비가 죽었을 때 발생할 시그널. 점수 정보를 함께 전달.
        [Signal]
        public delegate void ZombieDiedEventHandler(int scoreValue);

        // 플레이어 체력이 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void PlayerHealthChangedEventHandler(int currentHealth, int maxHealth);
        
        // 웨이브 정보가 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void WaveChangedEventHandler(int waveNumber);

        // 점수가 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void ScoreChangedEventHandler(int newScore);

        // 게임 상태가 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void GameStateChangedEventHandler(int newState);
        
        // 파워업이 수집되었을 때 발생할 시그널
        [Signal]
        public delegate void PowerupCollectedEventHandler();

        // 플레이어가 무기를 발사했을 때 발생할 시그널
        [Signal]
        public delegate void PlayerFiredWeaponEventHandler();

        // 좀비가 데미지를 받았을 때 발생할 시그널
        [Signal]
        public delegate void ZombieTookDamageEventHandler();

        // 플레이어가 데미지를 받았을 때 발생할 시그널
        [Signal]
        public delegate void PlayerTookDamageEventHandler();

        // 총알이 벽에 부딪혔을 때 발생할 시그널
        [Signal]
        public delegate void ProjectileHitWallEventHandler();

        // UI 관련 시그널들
        // 게임 시작 요청 시그널
        [Signal]
        public delegate void StartGameRequestedEventHandler();

        // 설정 화면 표시 요청 시그널
        [Signal]
        public delegate void ShowSettingsRequestedEventHandler();

        // 게임 종료 요청 시그널
        [Signal]
        public delegate void QuitGameRequestedEventHandler();

        // 게임 오버 시그널
        [Signal]
        public delegate void GameOverEventHandler();

        // 일시정지 토글 시그널
        [Signal]
        public delegate void PauseToggledEventHandler(bool isPaused);

        // 메인 메뉴 표시 요청 시그널
        [Signal]
        public delegate void ShowMainMenuRequestedEventHandler();

        // 카메라 쉐이크 이벤트들
        [Signal]
        public delegate void CameraShakeRequestedEventHandler(float intensity, float duration);

        // 사망 효과 이벤트들
        [Signal]
        public delegate void ZombieDeathEffectRequestedEventHandler(int zombieType, Vector2 position);

        // 화면 플래시 효과 이벤트
        [Signal]
        public delegate void ScreenFlashRequestedEventHandler();

        // 허기 시스템 이벤트들
        [Signal]
        public delegate void HungerChangedEventHandler(int currentHunger, int maxHunger);

        [Signal] 
        public delegate void StarvationStartedEventHandler();

        [Signal]
        public delegate void FoodConsumedEventHandler();

        public override void _Ready()
        {
            GD.Print("Events 이벤트 버스가 초기화되었습니다.");
        }
    }
}