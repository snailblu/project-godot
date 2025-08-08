using projectgodot.Constants;

namespace projectgodot.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// 값이 0보다 큰지 검사합니다.
        /// </summary>
        /// <param name="value">검사할 값</param>
        /// <returns>값이 양수이면 true, 아니면 false</returns>
        public static bool IsPositiveValue(int value)
        {
            return value > GameConstants.Validation.MINIMUM_POSITIVE_VALUE;
        }
        
        /// <summary>
        /// 값이 0보다 큰지 검사합니다.
        /// </summary>
        /// <param name="value">검사할 값</param>
        /// <returns>값이 양수이면 true, 아니면 false</returns>
        public static bool IsPositiveValue(float value)
        {
            return value > GameConstants.Validation.MINIMUM_POSITIVE_VALUE;
        }
        
        /// <summary>
        /// 값이 0보다 큰지 검사합니다.
        /// </summary>
        /// <param name="value">검사할 값</param>
        /// <returns>값이 양수이면 true, 아니면 false</returns>
        public static bool IsPositiveValue(double value)
        {
            return value > GameConstants.Validation.MINIMUM_POSITIVE_VALUE;
        }
        
        /// <summary>
        /// 체력 값이 유효한지 검사합니다 (0 이상).
        /// </summary>
        /// <param name="health">체력 값</param>
        /// <returns>유효하면 true, 아니면 false</returns>
        public static bool IsValidHealth(int health)
        {
            return health >= GameConstants.Validation.MINIMUM_POSITIVE_VALUE;
        }
        
        /// <summary>
        /// 최대 체력이 유효한지 검사합니다 (0보다 큰 값).
        /// </summary>
        /// <param name="maxHealth">최대 체력 값</param>
        /// <returns>유효하면 true, 아니면 false</returns>
        public static bool IsValidMaxHealth(int maxHealth)
        {
            return IsPositiveValue(maxHealth);
        }
        
        /// <summary>
        /// 컬렉션이 null이 아니고 비어있지 않은지 검사합니다.
        /// </summary>
        /// <typeparam name="T">컬렉션 요소 타입</typeparam>
        /// <param name="collection">검사할 컬렉션</param>
        /// <returns>유효하면 true, 아니면 false</returns>
        public static bool IsValidCollection<T>(System.Collections.Generic.ICollection<T> collection)
        {
            return collection != null && collection.Count > 0;
        }
    }
}