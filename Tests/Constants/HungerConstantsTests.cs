using NUnit.Framework;
using projectgodot.Constants;

namespace projectgodot.Tests
{
    [TestFixture]
    public class HungerConstantsTests
    {
        [Test]
        public void HungerConstants_ShouldHaveExpectedValues()
        {
            // Assert
            Assert.That(GameConstants.Hunger.DEFAULT_MAX_HUNGER, Is.EqualTo(100));
            Assert.That(GameConstants.Hunger.HUNGER_DECREASE_RATE, Is.EqualTo(2.0f));
            Assert.That(GameConstants.Hunger.STARVATION_DAMAGE_RATE, Is.EqualTo(5.0f));
            Assert.That(GameConstants.Hunger.FOOD_RESTORE_AMOUNT, Is.EqualTo(25));
        }

        [Test]
        public void HungerConstants_ShouldBePositiveValues()
        {
            // Assert
            Assert.That(GameConstants.Hunger.DEFAULT_MAX_HUNGER, Is.GreaterThan(0));
            Assert.That(GameConstants.Hunger.HUNGER_DECREASE_RATE, Is.GreaterThan(0));
            Assert.That(GameConstants.Hunger.STARVATION_DAMAGE_RATE, Is.GreaterThan(0));
            Assert.That(GameConstants.Hunger.FOOD_RESTORE_AMOUNT, Is.GreaterThan(0));
        }

        [Test]
        public void HungerConstants_FoodRestoreAmount_ShouldNotExceedMaxHunger()
        {
            // Assert - 음식 하나로 완전히 회복되지 않도록 보장
            Assert.That(GameConstants.Hunger.FOOD_RESTORE_AMOUNT, Is.LessThan(GameConstants.Hunger.DEFAULT_MAX_HUNGER));
        }
    }
}