using NUnit.Framework;
using projectgodot;
using System.Threading;

namespace projectgodot.Tests
{
    [TestFixture]
    public class PowerupLogicTests
    {
        private PowerupLogic _powerupLogic;

        [SetUp]
        public void SetUp()
        {
            _powerupLogic = new PowerupLogic();
        }

        [Test]
        public void PowerupLogic_WhenCreated_IsNotActive()
        {
            Assert.That(_powerupLogic.IsActive, Is.False);
        }

        [Test]
        public void PowerupLogic_DefaultValues_AreSetCorrectly()
        {
            Assert.That(_powerupLogic.DamageMultiplier, Is.EqualTo(2.0f));
            Assert.That(_powerupLogic.Duration, Is.EqualTo(5.0f));
        }

        [Test]
        public void PowerupLogic_WhenActivated_BecomesActive()
        {
            float originalDamage = 10f;
            
            _powerupLogic.Activate(originalDamage);
            
            Assert.That(_powerupLogic.IsActive, Is.True);
            Assert.That(_powerupLogic.GetOriginalDamage(), Is.EqualTo(originalDamage));
        }

        [Test]
        public void PowerupLogic_WhenAlreadyActive_ActivateDoesNothing()
        {
            float firstDamage = 10f;
            float secondDamage = 20f;
            
            _powerupLogic.Activate(firstDamage);
            Assert.That(_powerupLogic.GetOriginalDamage(), Is.EqualTo(firstDamage));
            
            // 이미 활성화된 상태에서 다시 활성화 시도
            _powerupLogic.Activate(secondDamage);
            Assert.That(_powerupLogic.GetOriginalDamage(), Is.EqualTo(firstDamage), "원래 값이 유지되어야 함");
        }

        [Test]
        public void PowerupLogic_CalculateDamage_ReturnsCorrectValue()
        {
            float baseDamage = 10f;
            float expectedDamage = baseDamage * _powerupLogic.DamageMultiplier;
            
            // 비활성화 상태에서는 기본 데미지 반환
            Assert.That(_powerupLogic.CalculateDamage(baseDamage), Is.EqualTo(baseDamage));
            
            // 활성화 후에는 증폭된 데미지 반환
            _powerupLogic.Activate(baseDamage);
            Assert.That(_powerupLogic.CalculateDamage(baseDamage), Is.EqualTo(expectedDamage));
        }

        [Test]
        public void PowerupLogic_CustomMultiplier_AppliesCorrectly()
        {
            float baseDamage = 15f;
            _powerupLogic.DamageMultiplier = 3.0f;
            
            _powerupLogic.Activate(baseDamage);
            
            float expectedDamage = baseDamage * 3.0f;
            Assert.That(_powerupLogic.CalculateDamage(baseDamage), Is.EqualTo(expectedDamage));
        }

        [Test]
        public void PowerupLogic_AfterDuration_BecomesInactive()
        {
            // 매우 짧은 지속시간으로 설정
            _powerupLogic.Duration = 0.05f; // 50ms
            _powerupLogic.Activate(10f);
            
            Assert.That(_powerupLogic.IsActive, Is.True, "활성화 직후에는 true여야 함");
            
            // 지속시간보다 조금 더 대기
            Thread.Sleep(60);
            
            // Update 호출하여 상태 갱신
            _powerupLogic.Update();
            
            Assert.That(_powerupLogic.IsActive, Is.False, "지속시간 후에는 false가 되어야 함");
        }

        [Test]
        public void PowerupLogic_Deactivate_ForcesEndOfEffect()
        {
            _powerupLogic.Activate(10f);
            Assert.That(_powerupLogic.IsActive, Is.True);
            
            _powerupLogic.Deactivate();
            Assert.That(_powerupLogic.IsActive, Is.False);
        }

        [Test]
        public void PowerupLogic_GetRemainingTime_ReturnsCorrectValue()
        {
            _powerupLogic.Duration = 1.0f; // 1초
            
            // 활성화 전
            Assert.That(_powerupLogic.GetRemainingTime(), Is.EqualTo(0f));
            
            // 활성화 직후
            _powerupLogic.Activate(10f);
            var remainingTime = _powerupLogic.GetRemainingTime();
            Assert.That(remainingTime, Is.GreaterThan(0.9f).And.LessThanOrEqualTo(1.0f));
        }

        [Test]
        public void PowerupLogic_AfterDeactivation_CalculateDamageReturnsBaseDamage()
        {
            float baseDamage = 10f;
            
            _powerupLogic.Activate(baseDamage);
            Assert.That(_powerupLogic.CalculateDamage(baseDamage), Is.EqualTo(baseDamage * _powerupLogic.DamageMultiplier));
            
            _powerupLogic.Deactivate();
            Assert.That(_powerupLogic.CalculateDamage(baseDamage), Is.EqualTo(baseDamage));
        }
    }
}