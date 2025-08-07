using NUnit.Framework;
using Godot;

namespace projectgodot.Tests
{
    [TestFixture]
    public class WaveManagerTests
    {
        [Test]
        public void Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var waveManager = new WaveManager();

            // Assert
            Assert.That(waveManager, Is.Not.Null);
        }

        [Test]
        public void NewWaveManager_StartsAtWaveZero_AndNotInWave()
        {
            // Arrange & Act
            var waveManager = new WaveManager();

            // Assert
            Assert.That(waveManager.CurrentWaveNumber, Is.EqualTo(0));
            Assert.That(waveManager.IsWaveActive, Is.False);
        }

        [Test]
        public void StartNextWave_IncrementsWaveNumber_AndActivatesWave()
        {
            // Arrange
            var waveManager = new WaveManager();

            // Act
            waveManager.StartNextWave();

            // Assert
            Assert.That(waveManager.CurrentWaveNumber, Is.EqualTo(1));
            Assert.That(waveManager.IsWaveActive, Is.True);
        }

        [Test]
        public void StartNextWave_MultipleCallsIncrementCorrectly()
        {
            // Arrange
            var waveManager = new WaveManager();

            // Act
            waveManager.StartNextWave(); // 웨이브 1
            waveManager.EndWave();
            waveManager.StartNextWave(); // 웨이브 2

            // Assert
            Assert.That(waveManager.CurrentWaveNumber, Is.EqualTo(2));
            Assert.That(waveManager.IsWaveActive, Is.True);
        }

        [Test]
        public void GetZombiesToSpawn_ForWaveOne_ReturnsCorrectAmount()
        {
            // Arrange
            var waveManager = new WaveManager();

            // Act
            waveManager.StartNextWave(); // 1 웨이브 시작

            // Assert
            // 1 웨이브는 5마리 (3 + 1*2 = 5)
            Assert.That(waveManager.GetZombiesToSpawnThisWave(), Is.EqualTo(5)); 
        }

        [Test]
        public void GetZombiesToSpawn_ForWaveTwo_ReturnsCorrectAmount()
        {
            // Arrange
            var waveManager = new WaveManager();

            // Act
            waveManager.StartNextWave(); // 1 웨이브
            waveManager.EndWave();
            waveManager.StartNextWave(); // 2 웨이브

            // Assert
            // 2 웨이브는 7마리 (3 + 2*2 = 7)
            Assert.That(waveManager.GetZombiesToSpawnThisWave(), Is.EqualTo(7));
        }

        [Test]
        public void EndWave_DeactivatesWave()
        {
            // Arrange
            var waveManager = new WaveManager();
            waveManager.StartNextWave();

            // Act
            waveManager.EndWave();

            // Assert
            Assert.That(waveManager.IsWaveActive, Is.False);
            Assert.That(waveManager.CurrentWaveNumber, Is.EqualTo(1)); // 웨이브 번호는 유지
        }
    }
}