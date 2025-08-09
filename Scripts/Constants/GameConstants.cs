namespace projectgodot.Constants
{
    public static class GameConstants
    {
        // 좀비 설정
        public static class Zombie
        {
            public const float DEFAULT_MOVE_SPEED = 900.0f;
            public const int DEFAULT_INITIAL_HEALTH = 30;
        }
        
        // 플레이어 설정
        public static class Player
        {
            public const float DEFAULT_MOVEMENT_SPEED = 8000.0f;
        }
        
        // 대시 설정  
        public static class Dash
        {
            public const float DEFAULT_DASH_SPEED = 1000f;
            public const float DEFAULT_DASH_DURATION = 0.2f;
        }
        
        // 무기 설정
        public static class Weapon
        {
            public const float DEFAULT_DAMAGE = 10f;
        }
        
        // 투사체 설정
        public static class Projectile
        {
            public const float DEFAULT_SPEED = 600.0f;
            public const int DEFAULT_DAMAGE = 10;
        }
        
        // 파워업 설정
        public static class Powerup
        {
            public const float DEFAULT_DAMAGE_MULTIPLIER = 2.0f;
            public const float DEFAULT_DURATION = 5.0f;
        }
        
        // 웨이브 설정
        public static class Wave
        {
            public const int TANK_ZOMBIE_WAVE_INTERVAL = 5;
        }
        
        // 스폰 설정
        public static class Spawn
        {
            public const float RANDOM_OFFSET_RANGE = 50f; // -50 ~ 50
            public const float RANDOM_OFFSET_TOTAL = 100f; // NextDouble() * 100 - 50
        }
        
        // 씬 경로 상수
        
        // 카메라 쉐이크 설정
        public static class CameraShake
        {
            public const float LIGHT_INTENSITY = 2.0f;    // 총 발사 시
            public const float MEDIUM_INTENSITY = 5.0f;   // 좀비 사망 시
            public const float HEAVY_INTENSITY = 10.0f;   // 플레이어 데미지 시
            public const float DASH_INTENSITY = 3.0f;     // 대시 시
            
            public const float LIGHT_DURATION = 0.1f;     // 짧은 쉐이크
            public const float MEDIUM_DURATION = 0.2f;    // 중간 쉐이크
            public const float HEAVY_DURATION = 0.4f;     // 긴 쉐이크
            public const float DASH_DURATION = 0.15f;     // 대시 쉐이크
            
            public const float SHAKE_FREQUENCY = 30.0f;   // 쉐이크 진동 주파수
        }

        public static class Scenes
        {
            public const string MAIN_MENU = "res://Scenes/UI/MainMenu.tscn";
            public const string GAME = "res://Scenes/Main/Game.tscn";
            public const string GAME_OVER = "res://Scenes/UI/GameOverScreen.tscn";
        }
        
        // 사망 이펙트 설정
        public static class DeathEffect
        {
            // 파티클 수량 설정
            public const int BASIC_PARTICLE_COUNT = 50;        // 기본 좀비
            public const int RUNNER_PARTICLE_COUNT = 30;       // 러너 좀비  
            public const int TANK_PARTICLE_COUNT = 100;        // 탱커 좀비
            
            // 파티클 속도 설정 (5-15픽셀 이동용으로 극도로 낮춤)
            public const float BASIC_VELOCITY_SCALE = 0.05f;   // 2.5-5 픽셀
            public const float RUNNER_VELOCITY_SCALE = 0.08f;  // 4-8 픽셀  
            public const float TANK_VELOCITY_SCALE = 0.03f;    // 1.5-3 픽셀
            
            // 파티클 지속 시간 (바닥에 오래 남아있도록)
            public const float EFFECT_DURATION = 10.0f;
            
            // 파티클 크기 설정
            public const float BASIC_SCALE_MIN = 2.0f;
            public const float BASIC_SCALE_MAX = 4.0f;
            public const float RUNNER_SCALE_MIN = 1.5f;
            public const float RUNNER_SCALE_MAX = 3.0f;
            public const float TANK_SCALE_MIN = 3.0f;
            public const float TANK_SCALE_MAX = 6.0f;
            
            // 파티클 물리 설정
            public const float SPREAD_ANGLE = 15.0f;       // 파티클 퍼짐 각도 (아래쪽만 분사되도록 축소)
            public const float GRAVITY_STRENGTH = 98.0f;   // 중력 강도
            
            // 화면 플래시 설정
            public const float FLASH_INTENSITY = 0.3f;        // 플래시 강도
            public const float FLASH_DURATION = 0.3f;         // 플래시 지속 시간
            
            // Z-Index 설정 (레이어 순서)
            public const int Z_INDEX = -10;                   // 뒤쪽 레이어로 렌더링
            
            // 파티클 물리 댐핑 설정 (빠르게 멈추도록 강화)
            public const float LINEAR_DAMP = 8.0f;            // 파티클 속도 감쇠
        }
        
        // 유효성 검사 상수
        // 총알 효과 설정
        public static class BulletEffects
        {
            // 총구 화염 효과
            public const int MUZZLE_FLASH_PARTICLE_COUNT = 15;
            public const float MUZZLE_FLASH_DURATION = 0.1f;
            public const float MUZZLE_FLASH_SCALE_MIN = 1.0f;
            public const float MUZZLE_FLASH_SCALE_MAX = 2.5f;
            public const float MUZZLE_FLASH_VELOCITY = 200.0f;
            
            // 총알 트레일 효과
            public const int BULLET_TRAIL_PARTICLE_COUNT = 20;
            public const float TRAIL_LIFETIME = 0.3f;
            public const float TRAIL_WIDTH = 2.0f;
            public const float TRAIL_FADE_SPEED = 3.0f;
            
            // 벽 충돌 스파크 효과  
            public const int WALL_HIT_SPARK_COUNT = 25;
            public const float SPARK_VELOCITY_MIN = 100.0f;
            public const float SPARK_VELOCITY_MAX = 300.0f;
            public const float SPARK_DURATION = 0.4f;
            public const float SPARK_SCALE_MIN = 0.5f;
            public const float SPARK_SCALE_MAX = 1.5f;
            public const float SPARK_GRAVITY = 98.0f;
        }

        public static class Validation
        {
            public const int MINIMUM_POSITIVE_VALUE = 0;
            public const double MINIMUM_PERCENTAGE = 0.0;
            public const double MAXIMUM_PERCENTAGE = 100.0;
        }
    }
}