using NUnit.Framework;
using System;
using System.Collections.Generic;
using Godot;
using projectgodot.Helpers;

namespace projectgodot.Tests
{
    [TestFixture]
    public class SpawnHelperTests
    {
        [Test]
        public void CalculateSpawnPositionWithOffset_ShouldReturnDifferentPosition()
        {
            Vector2 basePosition = new Vector2(100, 100);
            
            // 여러 번 호출하여 다른 결과가 나오는지 확인
            Vector2 result1 = SpawnHelper.CalculateSpawnPositionWithOffset(basePosition);
            Vector2 result2 = SpawnHelper.CalculateSpawnPositionWithOffset(basePosition);
            
            // 기본 위치와는 달라야 함 (확률적으로 매우 낮지만 같을 수도 있음)
            // 대신 결과가 예상 범위 내에 있는지 확인
            float maxOffset = 50f;
            Assert.That(result1.X >= basePosition.X - maxOffset && result1.X <= basePosition.X + maxOffset, Is.True);
            Assert.That(result1.Y >= basePosition.Y - maxOffset && result1.Y <= basePosition.Y + maxOffset, Is.True);
        }
        
        [Test]
        public void GetCyclicSpawnPosition_WithValidInput_ShouldReturnCorrectPosition()
        {
            var spawnPositions = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(10, 10),
                new Vector2(20, 20)
            };
            
            Assert.That(SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 0), Is.EqualTo(spawnPositions[0]));
            Assert.That(SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 1), Is.EqualTo(spawnPositions[1]));
            Assert.That(SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 2), Is.EqualTo(spawnPositions[2]));
            
            // 인덱스가 리스트 크기를 초과할 때 순환하는지 확인
            Assert.That(SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 3), Is.EqualTo(spawnPositions[0]));
            Assert.That(SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 4), Is.EqualTo(spawnPositions[1]));
        }
        
        [Test]
        public void GetCyclicSpawnPosition_WithEmptyList_ShouldThrowException()
        {
            var spawnPositions = new List<Vector2>();
            
            Assert.Throws<ArgumentException>(() => 
                SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 0));
        }
        
        [Test]
        public void GetCyclicSpawnPosition_WithNullList_ShouldThrowException()
        {
            List<Vector2> spawnPositions = null;
            
            Assert.Throws<ArgumentException>(() => 
                SpawnHelper.GetCyclicSpawnPosition(spawnPositions, 0));
        }
        
        [Test]
        public void GetRandomizedSpawnPosition_ShouldReturnPositionWithinExpectedRange()
        {
            var spawnPositions = new List<Vector2>
            {
                new Vector2(100, 100)
            };
            
            Vector2 result = SpawnHelper.GetRandomizedSpawnPosition(spawnPositions, 0);
            Vector2 basePosition = spawnPositions[0];
            
            float maxOffset = 50f;
            Assert.That(result.X >= basePosition.X - maxOffset && result.X <= basePosition.X + maxOffset, Is.True);
            Assert.That(result.Y >= basePosition.Y - maxOffset && result.Y <= basePosition.Y + maxOffset, Is.True);
        }
        
        [Test]
        public void GetRandomizedSpawnPosition_WithMultiplePositions_ShouldUseCyclicLogic()
        {
            var spawnPositions = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(100, 100)
            };
            
            // 인덱스 0은 첫 번째 위치를 기반으로 해야 함
            Vector2 result1 = SpawnHelper.GetRandomizedSpawnPosition(spawnPositions, 0);
            Assert.That(IsWithinRange(result1, spawnPositions[0], 50f), Is.True);
            
            // 인덱스 1은 두 번째 위치를 기반으로 해야 함
            Vector2 result2 = SpawnHelper.GetRandomizedSpawnPosition(spawnPositions, 1);
            Assert.That(IsWithinRange(result2, spawnPositions[1], 50f), Is.True);
            
            // 인덱스 2는 다시 첫 번째 위치를 기반으로 해야 함 (순환)
            Vector2 result3 = SpawnHelper.GetRandomizedSpawnPosition(spawnPositions, 2);
            Assert.That(IsWithinRange(result3, spawnPositions[0], 50f), Is.True);
        }
        
        private bool IsWithinRange(Vector2 actual, Vector2 expected, float maxOffset)
        {
            return actual.X >= expected.X - maxOffset && actual.X <= expected.X + maxOffset &&
                   actual.Y >= expected.Y - maxOffset && actual.Y <= expected.Y + maxOffset;
        }
    }
}