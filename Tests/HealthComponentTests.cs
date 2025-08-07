using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class HealthComponentTests
    {
        [Test]
        public void Constructor_WhenCreated_SetsMaxHealthAndCurrentHealth()
        {
            // Arrange & Act
            var healthComponent = new HealthComponent(100);

            // Assert
            Assert.That(healthComponent.MaxHealth, Is.EqualTo(100));
            Assert.That(healthComponent.CurrentHealth, Is.EqualTo(100));
            Assert.That(healthComponent.IsDead, Is.False);
        }

        [Test]
        public void TakeDamage_WhenCalled_ReducesHealth()
        {
            // Arrange
            var healthComponent = new HealthComponent(100);

            // Act
            healthComponent.TakeDamage(30);

            // Assert
            Assert.That(healthComponent.CurrentHealth, Is.EqualTo(70));
        }

        [Test]
        public void TakeDamage_HealthReachesZero_IsDeadReturnsTrue()
        {
            // Arrange
            var healthComponent = new HealthComponent(20);

            // Act
            healthComponent.TakeDamage(25); // 체력보다 많은 데미지

            // Assert
            Assert.That(healthComponent.CurrentHealth, Is.EqualTo(0)); // 0 이하로 내려가지 않게 처리
            Assert.That(healthComponent.IsDead, Is.True);
        }

        [Test]
        public void TakeDamage_NegativeDamage_DoesNothing()
        {
            // Arrange
            var healthComponent = new HealthComponent(100);

            // Act
            healthComponent.TakeDamage(-10);

            // Assert
            Assert.That(healthComponent.CurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void Heal_WhenCalled_IncreasesHealth()
        {
            // Arrange
            var healthComponent = new HealthComponent(100);
            healthComponent.TakeDamage(30); // 체력을 70으로 감소

            // Act
            healthComponent.Heal(20);

            // Assert
            Assert.That(healthComponent.CurrentHealth, Is.EqualTo(90));
        }

        [Test]
        public void Heal_ExceedsMaxHealth_ClampsToMaxHealth()
        {
            // Arrange
            var healthComponent = new HealthComponent(100);
            healthComponent.TakeDamage(30); // 체력을 70으로 감소

            // Act
            healthComponent.Heal(50); // 최대 체력을 초과하는 회복

            // Assert
            Assert.That(healthComponent.CurrentHealth, Is.EqualTo(100)); // 최대 체력으로 제한
        }
    }
}