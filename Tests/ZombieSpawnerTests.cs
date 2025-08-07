using NUnit.Framework;
using Godot;
using Moq;

namespace projectgodot.Tests
{
    [TestFixture]
    public class ZombieSpawnerTests
    {
        [Test]
        public void Constructor_WithValidFactory_CreatesInstance()
        {
            // Arrange & Act
            var mockFactory = new Mock<ISceneFactory>();
            var spawner = new ZombieSpawner(mockFactory.Object);

            // Assert
            Assert.That(spawner, Is.Not.Null);
        }

        [Test]
        public void SpawnZombie_WithNullScene_ReturnsNull()
        {
            // Arrange
            var mockFactory = new Mock<ISceneFactory>();
            var spawner = new ZombieSpawner(mockFactory.Object);

            // Act
            var result = spawner.SpawnZombie(null, Vector2.Zero);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void SpawnZombie_WithNullScene_DoesNotCallFactory()
        {
            // Arrange
            var mockFactory = new Mock<ISceneFactory>();
            var spawner = new ZombieSpawner(mockFactory.Object);

            // Act
            spawner.SpawnZombie(null, Vector2.Zero);

            // Assert
            // null 씬으로 호출했을 때는 팩토리가 호출되지 않아야 함
            mockFactory.Verify(
                f => f.CreateZombie(It.IsAny<PackedScene>(), It.IsAny<Vector2>()), 
                Times.Never
            );
        }
    }
}