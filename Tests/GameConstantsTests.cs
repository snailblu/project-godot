using NUnit.Framework;
using projectgodot.Constants;

namespace projectgodot.Tests
{
    [TestFixture]
    public class GameConstantsTests
    {
        [Test]
        public void ZombieConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Zombie.DEFAULT_MOVE_SPEED, Is.EqualTo(900.0f));
            Assert.That(GameConstants.Zombie.DEFAULT_INITIAL_HEALTH, Is.EqualTo(30));
        }
        
        [Test]
        public void PlayerConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Player.DEFAULT_MOVEMENT_SPEED, Is.EqualTo(8000.0f));
        }
        
        [Test]
        public void DashConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Dash.DEFAULT_DASH_SPEED, Is.EqualTo(1000f));
            Assert.That(GameConstants.Dash.DEFAULT_DASH_DURATION, Is.EqualTo(0.2f));
        }
        
        [Test]
        public void WeaponConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Weapon.DEFAULT_DAMAGE, Is.EqualTo(10f));
        }
        
        [Test]
        public void ProjectileConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Projectile.DEFAULT_SPEED, Is.EqualTo(600.0f));
            Assert.That(GameConstants.Projectile.DEFAULT_DAMAGE, Is.EqualTo(10));
        }
        
        [Test]
        public void PowerupConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Powerup.DEFAULT_DAMAGE_MULTIPLIER, Is.EqualTo(2.0f));
            Assert.That(GameConstants.Powerup.DEFAULT_DURATION, Is.EqualTo(5.0f));
        }
        
        [Test]
        public void WaveConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Wave.TANK_ZOMBIE_WAVE_INTERVAL, Is.EqualTo(5));
        }
        
        [Test]
        public void SpawnConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Spawn.RANDOM_OFFSET_RANGE, Is.EqualTo(50f));
            Assert.That(GameConstants.Spawn.RANDOM_OFFSET_TOTAL, Is.EqualTo(100f));
        }
        
        [Test]
        public void ValidationConstants_ShouldHaveExpectedValues()
        {
            Assert.That(GameConstants.Validation.MINIMUM_POSITIVE_VALUE, Is.EqualTo(0));
            Assert.That(GameConstants.Validation.MINIMUM_PERCENTAGE, Is.EqualTo(0.0));
            Assert.That(GameConstants.Validation.MAXIMUM_PERCENTAGE, Is.EqualTo(100.0));
        }
    }
}