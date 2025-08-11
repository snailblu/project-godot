using NUnit.Framework;
using System;

namespace projectgodot.Tests
{
    [TestFixture]
    public class HungerComponentTests
    {
        [Test]
        public void HungerComponent_WhenCreated_SetsDefaultValues()
        {
            // Arrange & Act
            var hunger = new HungerComponent(100);
            
            // Assert
            Assert.That(hunger.MaxHunger, Is.EqualTo(100));
            Assert.That(hunger.CurrentHunger, Is.EqualTo(100));
            Assert.That(hunger.IsStarving, Is.False);
        }

        [Test]
        public void DecreaseHunger_WithPositiveAmount_ReducesHunger()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            
            // Act
            hunger.DecreaseHunger(10);
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(90));
        }

        [Test]
        public void IsStarving_WhenHungerReachesZero_ReturnsTrue()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            
            // Act
            hunger.DecreaseHunger(100);
            
            // Assert
            Assert.That(hunger.IsStarving, Is.True);
        }

        [Test]
        public void DecreaseHunger_BelowZero_ClampsToZero()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            
            // Act
            hunger.DecreaseHunger(150); // More than max hunger
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(0));
            Assert.That(hunger.IsStarving, Is.True);
        }

        [Test]
        public void Eat_WithFoodValue_IncreasesHunger()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            hunger.DecreaseHunger(50); // Current hunger: 50
            
            // Act
            hunger.Eat(25);
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(75));
        }

        [Test]
        public void Eat_ExceedsMaxHunger_ClampsToMax()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            hunger.DecreaseHunger(10); // Current hunger: 90
            
            // Act
            hunger.Eat(50); // 90 + 50 = 140, but should clamp to 100
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(100));
        }

        [Test]
        public void HungerChanged_WhenHungerDecreases_RaisesEvent()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            int eventValue = -1;
            hunger.HungerChanged += (value) => eventValue = value;
            
            // Act
            hunger.DecreaseHunger(25);
            
            // Assert
            Assert.That(eventValue, Is.EqualTo(75));
        }

        [Test]
        public void HungerChanged_WhenEating_RaisesEvent()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            hunger.DecreaseHunger(50); // Set to 50
            int eventValue = -1;
            hunger.HungerChanged += (value) => eventValue = value;
            
            // Act
            hunger.Eat(20);
            
            // Assert
            Assert.That(eventValue, Is.EqualTo(70));
        }

        [Test]
        public void StarvationStarted_WhenHungerReachesZero_RaisesEvent()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            bool starvationEventRaised = false;
            hunger.StarvationStarted += () => starvationEventRaised = true;
            
            // Act
            hunger.DecreaseHunger(100);
            
            // Assert
            Assert.That(starvationEventRaised, Is.True);
        }

        [Test]
        public void ProcessHunger_WithDeltaTime_ReducesHungerOverTime()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            float deltaTime = 1.0f; // 1초
            float hungerRate = 2.0f; // 초당 2씩 감소
            
            // Act
            hunger.ProcessHunger(deltaTime, hungerRate);
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(98));
        }

        [Test]
        public void ProcessHunger_MultipleSeconds_AccumulatesHungerDecrease()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            float deltaTime = 0.5f; // 0.5초씩
            float hungerRate = 2.0f; // 초당 2씩 감소
            
            // Act
            hunger.ProcessHunger(deltaTime, hungerRate); // -1
            hunger.ProcessHunger(deltaTime, hungerRate); // -1
            hunger.ProcessHunger(deltaTime, hungerRate); // -1
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(97)); // 100 - 3 = 97
        }

        [Test]
        public void ProcessHunger_WithZeroDelta_DoesNotChangeHunger()
        {
            // Arrange
            var hunger = new HungerComponent(100);
            float deltaTime = 0.0f;
            float hungerRate = 2.0f;
            
            // Act
            hunger.ProcessHunger(deltaTime, hungerRate);
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(100));
        }
    }
}