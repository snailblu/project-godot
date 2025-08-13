using NUnit.Framework;
using projectgodot.Components;

namespace projectgodot.Tests
{
    /// <summary>
    /// PlayerState enum과 StateTransitionRequest에 대한 기본 테스트
    /// StateMachine은 Godot 의존성이 있어 통합 테스트에서만 가능
    /// </summary>
    [TestFixture]
    public class PlayerStateTests
    {
        [Test]
        public void PlayerState_EnumValues_ShouldBeCorrect()
        {
            Assert.That((int)PlayerState.Idle, Is.EqualTo(0));
            Assert.That((int)PlayerState.Moving, Is.EqualTo(1));
            Assert.That((int)PlayerState.Shooting, Is.EqualTo(2));
            Assert.That((int)PlayerState.TakingDamage, Is.EqualTo(3));
            Assert.That((int)PlayerState.Dead, Is.EqualTo(4));
        }

        [Test]
        public void StateTransitionRequest_Constructor_ShouldSetProperties()
        {
            var request = new StateTransitionRequest(PlayerState.Shooting, "Test reason", "test data");

            Assert.That(request.TargetState, Is.EqualTo(PlayerState.Shooting));
            Assert.That(request.Reason, Is.EqualTo("Test reason"));
            Assert.That(request.Data, Is.EqualTo("test data"));
        }

        [Test]
        public void StateTransitionRequest_ConstructorWithDefaults_ShouldSetDefaultValues()
        {
            var request = new StateTransitionRequest(PlayerState.Moving);

            Assert.That(request.TargetState, Is.EqualTo(PlayerState.Moving));
            Assert.That(request.Reason, Is.EqualTo(""));
            Assert.That(request.Data, Is.Null);
        }
    }
}