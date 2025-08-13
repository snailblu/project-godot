using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class HealthComponentTests
    {
        [Test]
        public void TakeDamage_ReducesHealth_Correctly()
        {
            // 준비: 100의 체력을 가진 컴포넌트를 만든다.
            var health = new HealthComponent(100);

            // 실행: 30의 데미지를 입힌다.
            health.TakeDamage(30);

            // 단언: 현재 체력은 70이어야 한다.
            Assert.That(health.CurrentHealth, Is.EqualTo(70));
        }

        [Test]
        public void TakeDamage_CannotGoBelowZero()
        {
            var health = new HealthComponent(20);
            health.TakeDamage(50); // 체력보다 큰 데미지
            Assert.That(health.CurrentHealth, Is.EqualTo(0)); // 체력은 0 밑으로 내려가면 안 된다.
        }

        [Test]
        public void IsDead_ReturnsTrue_WhenHealthIsZero()
        {
            var health = new HealthComponent(10);
            health.TakeDamage(10);
            Assert.That(health.IsDead, Is.True); // 체력이 0이 되면, IsDead는 true여야 한다.
        }
    }
}