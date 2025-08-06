using NUnit.Framework;
using Godot;

namespace projectgodot.Tests
{
    [TestFixture]
    public class PlayerMovementTests
    {
        [Test]
        public void CalculateVelocity_GivenDirectionAndSpeed_ReturnsCorrectVelocity()
        {
            // Arrange
            var movement = new PlayerMovement();
            var direction = new Vector2(1, 1).Normalized(); // 대각선 방향 정규화
            float speed = 300.0f;
            float delta = 1.0f; // 계산 편의를 위해 1초 경과로 가정

            // Act
            var velocity = movement.CalculateVelocity(direction, speed, delta);

            // Assert
            Assert.That(velocity.X, Is.EqualTo(direction.X * speed * delta).Within(0.001));
            Assert.That(velocity.Y, Is.EqualTo(direction.Y * speed * delta).Within(0.001));
        }

        [Test]
        public void CalculateVelocity_GivenZeroDirection_ReturnsZeroVelocity()
        {
            // Arrange
            var movement = new PlayerMovement();
            var direction = Vector2.Zero;
            float speed = 300.0f;
            float delta = 1.0f;

            // Act
            var velocity = movement.CalculateVelocity(direction, speed, delta);

            // Assert
            Assert.That(velocity, Is.EqualTo(Vector2.Zero));
        }
    }
}