using NUnit.Framework;
using Godot;

namespace projectgodot.Tests
{
    [TestFixture]
    public class ZombieAIComponentTests
    {
        [Test]
        public void CalculateDirection_ToPlayer_ReturnsCorrectNormalizedVector()
        {
            // Arrange
            var aiComponent = new ZombieAIComponent();
            var zombiePosition = Vector2.Zero;
            var playerPosition = new Vector2(10, 0); // 좀비의 오른쪽에 플레이어

            // Act
            var direction = aiComponent.CalculateDirection(zombiePosition, playerPosition);

            // Assert
            Assert.That(direction, Is.EqualTo(new Vector2(1, 0)));
        }

        [Test]
        public void CalculateDirection_DiagonalTarget_ReturnsNormalizedVector()
        {
            // Arrange
            var aiComponent = new ZombieAIComponent();
            var zombiePosition = Vector2.Zero;
            var playerPosition = new Vector2(3, 4); // 3:4:5 직각삼각형

            // Act
            var direction = aiComponent.CalculateDirection(zombiePosition, playerPosition);

            // Assert - 정규화된 벡터는 (0.6, 0.8)이어야 함
            Assert.That(direction.X, Is.EqualTo(0.6f).Within(0.001f));
            Assert.That(direction.Y, Is.EqualTo(0.8f).Within(0.001f));
        }

        [Test]
        public void CalculateDirection_SamePosition_ReturnsZeroVector()
        {
            // Arrange
            var aiComponent = new ZombieAIComponent();
            var zombiePosition = new Vector2(5, 5);
            var playerPosition = new Vector2(5, 5); // 같은 위치

            // Act
            var direction = aiComponent.CalculateDirection(zombiePosition, playerPosition);

            // Assert
            Assert.That(direction, Is.EqualTo(Vector2.Zero));
        }

        [Test]
        public void CalculateDirection_LeftTarget_ReturnsLeftVector()
        {
            // Arrange
            var aiComponent = new ZombieAIComponent();
            var zombiePosition = new Vector2(0, 0);
            var playerPosition = new Vector2(-10, 0); // 좀비의 왼쪽에 플레이어

            // Act
            var direction = aiComponent.CalculateDirection(zombiePosition, playerPosition);

            // Assert
            Assert.That(direction, Is.EqualTo(new Vector2(-1, 0)));
        }
    }
}