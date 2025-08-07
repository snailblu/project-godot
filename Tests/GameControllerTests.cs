using NUnit.Framework;
using Godot;
using Moq;
using System.Collections.Generic;

namespace projectgodot.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _gameController;
        private Mock<IZombieSpawner> _mockSpawner;
        private WaveManager _waveManager;

        [SetUp]
        public void SetUp()
        {
            _mockSpawner = new Mock<IZombieSpawner>();
            _waveManager = new WaveManager();
            _gameController = new GameController(_waveManager, _mockSpawner.Object);
        }

        [Test]
        public void Constructor_WithValidDependencies_InitializesCorrectly()
        {
            // Assert
            Assert.That(_gameController, Is.Not.Null);
            Assert.That(_gameController.ZombiesRemainingInWave, Is.EqualTo(0));
        }

        [Test]
        public void StartWave_SpawnsCorrectNumberOfZombies()
        {
            // Arrange
            var spawnPositions = new List<Vector2> 
            { 
                new Vector2(100, 100), 
                new Vector2(200, 200) 
            };

            // Act
            _gameController.StartWave(spawnPositions);

            // Assert
            Assert.That(_gameController.ZombiesRemainingInWave, Is.EqualTo(5)); // 1웨이브 = 5마리
            
            // ZombieSpawner가 5번 호출되었는지 확인
            _mockSpawner.Verify(
                s => s.SpawnZombie(It.IsAny<PackedScene>(), It.IsAny<Vector2>()), 
                Times.Exactly(5)
            );
        }

        [Test]
        public void OnZombieDied_ReducesZombieCount()
        {
            // Arrange
            var spawnPositions = new List<Vector2> { new Vector2(100, 100) };
            _gameController.StartWave(spawnPositions);

            // Act
            _gameController.OnZombieDied();

            // Assert
            Assert.That(_gameController.ZombiesRemainingInWave, Is.EqualTo(4));
        }

        [Test]
        public void OnZombieDied_WhenAllZombiesDead_EndsWave()
        {
            // Arrange
            var spawnPositions = new List<Vector2> { new Vector2(100, 100) };
            _gameController.StartWave(spawnPositions);
            
            bool waveEnded = false;
            _gameController.WaveCleared += () => waveEnded = true;

            // Act - 모든 좀비를 죽임 (5마리)
            for (int i = 0; i < 5; i++)
            {
                _gameController.OnZombieDied();
            }

            // Assert
            Assert.That(_gameController.ZombiesRemainingInWave, Is.EqualTo(0));
            Assert.That(waveEnded, Is.True);
        }

        [Test]
        public void OnZombieDied_WhenZombiesRemaining_DoesNotEndWave()
        {
            // Arrange
            var spawnPositions = new List<Vector2> { new Vector2(100, 100) };
            _gameController.StartWave(spawnPositions);
            
            bool waveEnded = false;
            _gameController.WaveCleared += () => waveEnded = true;

            // Act - 1마리만 죽임
            _gameController.OnZombieDied();

            // Assert
            Assert.That(_gameController.ZombiesRemainingInWave, Is.EqualTo(4));
            Assert.That(waveEnded, Is.False);
        }
    }
}