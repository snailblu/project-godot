using NUnit.Framework;
using projectgodot;
using projectgodot.Scripts.Interfaces;
using projectgodot.Components;

namespace projectgodot.Tests
{
    [TestFixture]
    public class InterfaceBasedTests
    {
        [Test]
        public void WeaponComponent_ImplementsIWeapon()
        {
            // Arrange & Act
            var weapon = new WeaponComponent(0.5f);
            
            // Assert
            Assert.That(weapon, Is.InstanceOf<IWeapon>());
        }
        
        [Test]
        public void IWeapon_CanBeUsedPolymorphically()
        {
            // Arrange
            IWeapon weapon = new WeaponComponent(0.5f);
            bool eventFired = false;
            weapon.OnShoot += () => eventFired = true;
            
            // Act
            weapon.Shoot();
            
            // Assert
            Assert.That(eventFired, Is.True);
            Assert.That(weapon.CanShoot(), Is.False); // Should be on cooldown
        }
        
        [Test]
        public void HealthComponent_ImplementsIHealth()
        {
            // Arrange & Act
            var health = new HealthComponent(100);
            
            // Assert
            Assert.That(health, Is.InstanceOf<IHealth>());
            Assert.That(health.MaxHealth, Is.EqualTo(100));
            Assert.That(health.CurrentHealth, Is.EqualTo(100));
            Assert.That(health.IsDead, Is.False);
        }
        
        [Test]
        public void IHealth_CanBeUsedPolymorphically()
        {
            // Arrange
            IHealth health = new HealthComponent(50);
            bool deathEventFired = false;
            bool healthChangedEventFired = false;
            
            health.Died += () => deathEventFired = true;
            health.HealthChanged += (newHealth) => healthChangedEventFired = true;
            
            // Act
            health.TakeDamage(30);
            
            // Assert
            Assert.That(health.CurrentHealth, Is.EqualTo(20));
            Assert.That(healthChangedEventFired, Is.True);
            Assert.That(deathEventFired, Is.False);
            Assert.That(health.IsDead, Is.False);
            
            // Act - Take lethal damage
            health.TakeDamage(20);
            
            // Assert
            Assert.That(health.CurrentHealth, Is.EqualTo(0));
            Assert.That(health.IsDead, Is.True);
            Assert.That(deathEventFired, Is.True);
        }
        
        [Test]
        public void PlayerMovement_ImplementsIMovement()
        {
            // Arrange & Act
            var movement = new PlayerMovement();
            
            // Assert
            Assert.That(movement, Is.InstanceOf<IMovement>());
        }
        
        [Test]
        public void IMovement_CalculatesVelocityCorrectly()
        {
            // Arrange
            IMovement movement = new PlayerMovement();
            var direction = new Godot.Vector2(1, 0); // Right direction
            float speed = 100f;
            float delta = 0.016f; // ~60 FPS
            
            // Act
            var velocity = movement.CalculateVelocity(direction, speed, delta);
            
            // Assert
            Assert.That(velocity.X, Is.EqualTo(direction.X * speed * delta));
            Assert.That(velocity.Y, Is.EqualTo(direction.Y * speed * delta));
        }
        
        [Test]
        public void HungerComponent_ImplementsIHunger()
        {
            // Arrange & Act
            var hunger = new HungerComponent(100);
            
            // Assert
            Assert.That(hunger, Is.InstanceOf<IHunger>());
            Assert.That(hunger.MaxHunger, Is.EqualTo(100));
            Assert.That(hunger.CurrentHunger, Is.EqualTo(100));
            Assert.That(hunger.IsStarving, Is.False);
        }
        
        [Test]
        public void IHunger_CanBeUsedPolymorphically()
        {
            // Arrange
            IHunger hunger = new HungerComponent(50);
            bool hungerChangedEventFired = false;
            bool starvationStartedEventFired = false;
            
            hunger.HungerChanged += (newHunger) => hungerChangedEventFired = true;
            hunger.StarvationStarted += () => starvationStartedEventFired = true;
            
            // Act
            hunger.DecreaseHunger(30);
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(20));
            Assert.That(hungerChangedEventFired, Is.True);
            Assert.That(hunger.IsStarving, Is.False);
            Assert.That(starvationStartedEventFired, Is.False);
            
            // Act - Cause starvation
            hunger.DecreaseHunger(20);
            
            // Assert
            Assert.That(hunger.CurrentHunger, Is.EqualTo(0));
            Assert.That(hunger.IsStarving, Is.True);
            Assert.That(starvationStartedEventFired, Is.True);
        }
    }
}