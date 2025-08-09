using NUnit.Framework;
using Godot;
using projectgodot;
using projectgodot.Constants;

namespace projectgodot.Tests
{
    [TestFixture]
    public class DeathEffectTests
    {
        [Test]
        public void ZombieType_Basic_SetsCorrectParticleCount()
        {
            // Arrange & Act
            var zombieType = ZombieType.Basic;
            var expectedParticleCount = GameConstants.DeathEffect.BASIC_PARTICLE_COUNT;
            
            // Assert
            Assert.That(expectedParticleCount, Is.EqualTo(50));
            Assert.That(zombieType, Is.EqualTo(ZombieType.Basic));
        }

        [Test]
        public void ZombieType_Runner_SetsCorrectParticleCount()
        {
            // Arrange & Act
            var zombieType = ZombieType.Runner;
            var expectedParticleCount = GameConstants.DeathEffect.RUNNER_PARTICLE_COUNT;
            
            // Assert
            Assert.That(expectedParticleCount, Is.EqualTo(30));
            Assert.That(zombieType, Is.EqualTo(ZombieType.Runner));
        }

        [Test]
        public void ZombieType_Tank_SetsCorrectParticleCount()
        {
            // Arrange & Act
            var zombieType = ZombieType.Tank;
            var expectedParticleCount = GameConstants.DeathEffect.TANK_PARTICLE_COUNT;
            
            // Assert
            Assert.That(expectedParticleCount, Is.EqualTo(100));
            Assert.That(zombieType, Is.EqualTo(ZombieType.Tank));
        }

        [Test]
        public void VelocityScale_Constants_AreCorrectlyDefined()
        {
            // Assert - 5-15픽셀 이동용으로 극도로 낮춘 값들
            Assert.That(GameConstants.DeathEffect.BASIC_VELOCITY_SCALE, Is.EqualTo(0.05f));
            Assert.That(GameConstants.DeathEffect.RUNNER_VELOCITY_SCALE, Is.EqualTo(0.08f));
            Assert.That(GameConstants.DeathEffect.TANK_VELOCITY_SCALE, Is.EqualTo(0.03f));
        }

        [Test]
        public void EffectDuration_IsPositive()
        {
            // Assert
            Assert.That(GameConstants.DeathEffect.EFFECT_DURATION, Is.GreaterThan(0.0f));
            Assert.That(GameConstants.DeathEffect.EFFECT_DURATION, Is.EqualTo(10.0f));
        }

        [Test]
        public void FlashEffect_Constants_AreValid()
        {
            // Assert
            Assert.That(GameConstants.DeathEffect.FLASH_INTENSITY, Is.GreaterThan(0.0f));
            Assert.That(GameConstants.DeathEffect.FLASH_INTENSITY, Is.LessThanOrEqualTo(1.0f));
            Assert.That(GameConstants.DeathEffect.FLASH_DURATION, Is.GreaterThan(0.0f));
            Assert.That(GameConstants.DeathEffect.FLASH_DURATION, Is.EqualTo(0.3f));
        }

        [Test]
        public void ParticleScale_Constants_AreCorrectlyDefined()
        {
            // Assert - 기본 좀비 크기
            Assert.That(GameConstants.DeathEffect.BASIC_SCALE_MIN, Is.EqualTo(2.0f));
            Assert.That(GameConstants.DeathEffect.BASIC_SCALE_MAX, Is.EqualTo(4.0f));
            
            // Assert - 러너 좀비 크기
            Assert.That(GameConstants.DeathEffect.RUNNER_SCALE_MIN, Is.EqualTo(1.5f));
            Assert.That(GameConstants.DeathEffect.RUNNER_SCALE_MAX, Is.EqualTo(3.0f));
            
            // Assert - 탱크 좀비 크기
            Assert.That(GameConstants.DeathEffect.TANK_SCALE_MIN, Is.EqualTo(3.0f));
            Assert.That(GameConstants.DeathEffect.TANK_SCALE_MAX, Is.EqualTo(6.0f));
        }

        [Test]
        public void PhysicsConstants_AreValid()
        {
            // Assert
            Assert.That(GameConstants.DeathEffect.SPREAD_ANGLE, Is.GreaterThan(0.0f));
            Assert.That(GameConstants.DeathEffect.GRAVITY_STRENGTH, Is.GreaterThan(0.0f));
            Assert.That(GameConstants.DeathEffect.SPREAD_ANGLE, Is.EqualTo(15.0f));
            Assert.That(GameConstants.DeathEffect.GRAVITY_STRENGTH, Is.EqualTo(98.0f));
        }

        [Test]
        public void ZombieType_Enum_HasAllExpectedValues()
        {
            // Arrange
            var expectedValues = new[] { ZombieType.Basic, ZombieType.Runner, ZombieType.Tank };
            
            // Act
            var actualValues = System.Enum.GetValues<ZombieType>();
            
            // Assert
            Assert.That(actualValues.Length, Is.EqualTo(3));
            Assert.That(actualValues, Is.EquivalentTo(expectedValues));
        }

        [Test]
        public void ParticleCount_DiffersByZombieType()
        {
            // Arrange
            var basicCount = GameConstants.DeathEffect.BASIC_PARTICLE_COUNT;
            var runnerCount = GameConstants.DeathEffect.RUNNER_PARTICLE_COUNT;  
            var tankCount = GameConstants.DeathEffect.TANK_PARTICLE_COUNT;
            
            // Assert - 각 좀비 타입별로 다른 파티클 수량
            Assert.That(basicCount, Is.Not.EqualTo(runnerCount));
            Assert.That(runnerCount, Is.Not.EqualTo(tankCount));
            Assert.That(basicCount, Is.Not.EqualTo(tankCount));
            
            // Tank 좀비가 가장 많은 파티클을 가져야 함
            Assert.That(tankCount, Is.GreaterThan(basicCount));
            Assert.That(tankCount, Is.GreaterThan(runnerCount));
        }
    }
}