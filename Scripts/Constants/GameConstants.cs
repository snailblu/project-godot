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
        
        // 유효성 검사 상수
        public static class Validation
        {
            public const int MINIMUM_POSITIVE_VALUE = 0;
            public const double MINIMUM_PERCENTAGE = 0.0;
            public const double MAXIMUM_PERCENTAGE = 100.0;
        }
    }
}