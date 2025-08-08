using NUnit.Framework;
using projectgodot;
using System.Threading;

namespace projectgodot.Tests
{
    [TestFixture]
    public class DashLogicTests
    {
        private DashLogic _dashLogic;

        [SetUp]
        public void SetUp()
        {
            _dashLogic = new DashLogic();
        }

        [Test]
        public void DashLogic_WhenCreated_IsDashingIsFalse()
        {
            Assert.That(_dashLogic.IsDashing, Is.False);
        }
        
        [Test]
        public void DashLogic_WhenStartDashCalled_IsDashingBecomesTrue()
        {
            _dashLogic.StartDash();
            
            Assert.That(_dashLogic.IsDashing, Is.True);
        }
        
        [Test]
        public void DashLogic_WhenAlreadyDashing_StartDashDoesNothing()
        {
            // 첫 번째 대시 시작
            _dashLogic.StartDash();
            Assert.That(_dashLogic.IsDashing, Is.True);
            
            // 두 번째 대시 시도 (무시되어야 함)
            _dashLogic.StartDash();
            Assert.That(_dashLogic.IsDashing, Is.True, "여전히 대시 중이어야 함");
        }
        
        [Test]
        public void DashLogic_DefaultValues_AreSetCorrectly()
        {
            Assert.That(_dashLogic.DashSpeed, Is.EqualTo(1000f));
            Assert.That(_dashLogic.DashDuration, Is.EqualTo(0.2f));
        }
        
        [Test]
        public void DashLogic_CustomValues_CanBeSet()
        {
            _dashLogic.DashSpeed = 1500f;
            _dashLogic.DashDuration = 0.3f;
            
            Assert.That(_dashLogic.DashSpeed, Is.EqualTo(1500f));
            Assert.That(_dashLogic.DashDuration, Is.EqualTo(0.3f));
        }

        [Test]
        public void DashLogic_AfterDuration_IsDashingBecomesFalse()
        {
            // 매우 짧은 지속시간으로 설정
            _dashLogic.DashDuration = 0.05f; // 50ms
            _dashLogic.StartDash();
            
            Assert.That(_dashLogic.IsDashing, Is.True, "대시 시작 직후에는 true여야 함");
            
            // 지속시간보다 조금 더 대기
            Thread.Sleep(60);
            
            // Update 호출하여 상태 갱신
            _dashLogic.Update(0.06f);
            
            Assert.That(_dashLogic.IsDashing, Is.False, "지속시간 후에는 false가 되어야 함");
        }

        [Test]
        public void DashLogic_EndDash_ForcesEndOfDash()
        {
            _dashLogic.StartDash();
            Assert.That(_dashLogic.IsDashing, Is.True);
            
            _dashLogic.EndDash();
            Assert.That(_dashLogic.IsDashing, Is.False);
        }

        [Test]
        public void DashLogic_GetRemainingTime_ReturnsCorrectValue()
        {
            _dashLogic.DashDuration = 0.5f;
            
            // 대시 시작 전
            Assert.That(_dashLogic.GetRemainingTime(), Is.EqualTo(0f));
            
            // 대시 시작 직후
            _dashLogic.StartDash();
            var remainingTime = _dashLogic.GetRemainingTime();
            Assert.That(remainingTime, Is.GreaterThan(0.4f).And.LessThanOrEqualTo(0.5f));
        }
    }
}