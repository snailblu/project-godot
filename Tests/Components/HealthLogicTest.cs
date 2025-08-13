// HealthLogicTest.cs (NUnit 테스트)
using NUnit.Framework;

[TestFixture]
public class HealthLogicTest
{
    [Test]
    public void TakeDamage_ReducesHealth_Correctly()
    {
        // 준비
        var healthLogic = new HealthLogic(100f);

        // 실행
        healthLogic.TakeDamage(30f);

        // 단언
        Assert.That(healthLogic.CurrentHealth, Is.EqualTo(70f));
    }

    [Test]
    public void DiedEvent_IsRaised_WhenHealthReachesZero()
    {
        // 준비
        var healthLogic = new HealthLogic(20f);
        bool wasDiedCalled = false;
        // Died 이벤트가 발생하면, wasDiedCalled를 true로 바꾼다.
        healthLogic.Died += () => wasDiedCalled = true;

        // 실행
        healthLogic.TakeDamage(20f);

        // 단언
        Assert.That(wasDiedCalled, Is.True, "Died 이벤트가 호출되지 않았습니다.");
    }
}