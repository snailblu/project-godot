using NUnit.Framework;
using projectgodot;

namespace projectgodot.Tests
{
    [TestFixture]
    public class WeaponComponentTests
    {
        [Test]
        public void CanShoot_WhenCreated_ReturnsTrue()
        {
            // Arrange
            var weapon = new WeaponComponent(cooldown: 0.5f);

            // Act & Assert
            Assert.That(weapon.CanShoot(), Is.True);
        }

        [Test]
        public void CanShoot_AfterShooting_ReturnsFalse()
        {
            // Arrange
            var weapon = new WeaponComponent(cooldown: 0.5f);

            // Act
            weapon.Shoot();

            // Assert
            Assert.That(weapon.CanShoot(), Is.False);
        }

        [Test]
        public void CanShoot_AfterCooldownElapsed_ReturnsTrue()
        {
            // Arrange
            var weapon = new WeaponComponent(cooldown: 0.5f);
            weapon.Shoot(); // 발사하여 쿨다운 시작

            // Act
            weapon.Process(deltaTime: 0.6f); // 쿨다운 시간보다 더 오랜 시간 경과

            // Assert
            Assert.That(weapon.CanShoot(), Is.True);
        }

        [Test]
        public void CanShoot_BeforeCooldownElapsed_ReturnsFalse()
        {
            // Arrange
            var weapon = new WeaponComponent(cooldown: 0.5f);
            weapon.Shoot(); // 발사하여 쿨다운 시작

            // Act
            weapon.Process(deltaTime: 0.3f); // 쿨다운 시간보다 적은 시간 경과

            // Assert
            Assert.That(weapon.CanShoot(), Is.False);
        }

        [Test]
        public void Shoot_WhenCannotShoot_DoesNotTriggerEvent()
        {
            // Arrange
            var weapon = new WeaponComponent(cooldown: 0.5f);
            bool eventTriggered = false;
            weapon.OnShoot += () => eventTriggered = true;

            weapon.Shoot(); // 첫 번째 발사
            eventTriggered = false; // 이벤트 플래그 리셋

            // Act - 쿨다운 중에 다시 발사 시도
            weapon.Shoot();

            // Assert
            Assert.That(eventTriggered, Is.False);
        }

        [Test]
        public void Shoot_WhenCanShoot_TriggersOnShootEvent()
        {
            // Arrange
            var weapon = new WeaponComponent(cooldown: 0.5f);
            bool eventTriggered = false;
            weapon.OnShoot += () => eventTriggered = true;

            // Act
            weapon.Shoot();

            // Assert
            Assert.That(eventTriggered, Is.True);
        }
    }
}