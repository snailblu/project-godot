using System;

namespace projectgodot.Utils
{
    public static class GodotLogger
    {
        private static bool IsTestEnvironment => 
            Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") == "true";
        
        /// <summary>
        /// 테스트 환경에서 안전한 로깅을 제공합니다.
        /// Godot 환경이 아닐 때는 예외를 무시합니다.
        /// </summary>
        /// <param name="message">로그 메시지</param>
        public static void SafePrint(string message)
        {
            if (IsTestEnvironment)
                return;
                
            try
            {
                Godot.GD.Print(message);
            }
            catch
            {
                // Godot 환경이 아닐 때는 무시
            }
        }
        
        /// <summary>
        /// 데미지 로그를 출력합니다.
        /// </summary>
        /// <param name="amount">데미지 양</param>
        /// <param name="currentHealth">현재 체력</param>
        /// <param name="maxHealth">최대 체력</param>
        public static void LogDamage(int amount, int currentHealth, int maxHealth)
        {
            SafePrint($"Zombie took {amount} damage! Current health: {currentHealth}/{maxHealth}");
        }
        
        /// <summary>
        /// 죽음 로그를 출력합니다.
        /// </summary>
        public static void LogDeath()
        {
            SafePrint("Zombie DIED!");
        }
        
        /// <summary>
        /// 일반적인 게임 상태 로그를 출력합니다.
        /// </summary>
        /// <param name="message">로그 메시지</param>
        public static void LogGameState(string message)
        {
            SafePrint($"[GameState] {message}");
        }
        
        /// <summary>
        /// 디버그 정보를 로그로 출력합니다.
        /// </summary>
        /// <param name="message">디버그 메시지</param>
        public static void LogDebug(string message)
        {
            SafePrint($"[DEBUG] {message}");
        }
    }
}