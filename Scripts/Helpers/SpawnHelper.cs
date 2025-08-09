using Godot;
using System;
using projectgodot.Constants;

namespace projectgodot.Helpers
{
    public static class SpawnHelper
    {
        private static readonly Random _random = new Random();
        
        /// <summary>
        /// 기본 스폰 위치에 랜덤 오프셋을 적용하여 최종 스폰 위치를 계산합니다.
        /// </summary>
        /// <param name="basePosition">기본 스폰 위치</param>
        /// <returns>오프셋이 적용된 최종 스폰 위치</returns>
        public static Vector2 CalculateSpawnPositionWithOffset(Vector2 basePosition)
        {
            float offsetX = (float)(_random.NextDouble() * GameConstants.Spawn.RANDOM_OFFSET_TOTAL - GameConstants.Spawn.RANDOM_OFFSET_RANGE);
            float offsetY = (float)(_random.NextDouble() * GameConstants.Spawn.RANDOM_OFFSET_TOTAL - GameConstants.Spawn.RANDOM_OFFSET_RANGE);
            
            return basePosition + new Vector2(offsetX, offsetY);
        }
        
        /// <summary>
        /// 스폰 위치 리스트에서 인덱스를 순환하며 위치를 선택합니다.
        /// </summary>
        /// <param name="spawnPositions">스폰 위치 리스트</param>
        /// <param name="index">현재 인덱스</param>
        /// <returns>선택된 스폰 위치</returns>
        public static Vector2 GetCyclicSpawnPosition(System.Collections.Generic.List<Vector2> spawnPositions, int index)
        {
            if (spawnPositions == null || spawnPositions.Count == 0)
                throw new ArgumentException("스폰 위치 리스트가 비어있습니다", nameof(spawnPositions));
                
            return spawnPositions[index % spawnPositions.Count];
        }
        
        /// <summary>
        /// 기본 스폰 위치에서 랜덤 오프셋이 적용된 최종 스폰 위치를 한 번에 계산합니다.
        /// </summary>
        /// <param name="spawnPositions">스폰 위치 리스트</param>
        /// <param name="index">현재 인덱스</param>
        /// <returns>오프셋이 적용된 최종 스폰 위치</returns>
        public static Vector2 GetRandomizedSpawnPosition(System.Collections.Generic.List<Vector2> spawnPositions, int index)
        {
            Vector2 basePosition = GetCyclicSpawnPosition(spawnPositions, index);
            return CalculateSpawnPositionWithOffset(basePosition);
        }
    }
}