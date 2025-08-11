using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class HUDTests
    {
        [Test]
        public void UpdateHealthData_CalculatesCorrectPercentage()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.CalculateHealthPercentage(75, 100);

            // Assert
            Assert.That(result, Is.EqualTo(75.0));
        }

        [Test]
        public void UpdateHealthData_WithZeroMaxHealth_ReturnsZero()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.CalculateHealthPercentage(50, 0);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void UpdateHealthData_WithNegativeCurrentHealth_ReturnsZero()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.CalculateHealthPercentage(-10, 100);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void FormatScoreText_ReturnsCorrectFormat()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.FormatScoreText(1250);

            // Assert
            Assert.That(result, Is.EqualTo("Score: 1250"));
        }

        [Test]
        public void FormatWaveText_ReturnsCorrectFormat()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.FormatWaveText(5);

            // Assert
            Assert.That(result, Is.EqualTo("Wave: 5"));
        }

        [Test]
        public void FormatScoreText_WithZeroScore_ReturnsCorrectFormat()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.FormatScoreText(0);

            // Assert
            Assert.That(result, Is.EqualTo("Score: 0"));
        }

        [Test]
        public void CalculateHungerPercentage_WithValidValues_ReturnsCorrectPercentage()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.CalculateHungerPercentage(75, 100);

            // Assert
            Assert.That(result, Is.EqualTo(75.0));
        }

        [Test]
        public void CalculateHungerPercentage_WithZeroHunger_ReturnsZero()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.CalculateHungerPercentage(0, 100);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateHungerPercentage_WithZeroMaxHunger_ReturnsZero()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.CalculateHungerPercentage(50, 0);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void FormatHungerText_ReturnsCorrectFormat()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.FormatHungerText(75, 100);

            // Assert
            Assert.That(result, Is.EqualTo("Hunger: 75/100"));
        }

        [Test]
        public void IsStarving_WithZeroHunger_ReturnsTrue()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.IsStarving(0);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsStarving_WithPositiveHunger_ReturnsFalse()
        {
            // Arrange
            var hudLogic = new HUDLogic();

            // Act
            var result = hudLogic.IsStarving(50);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}