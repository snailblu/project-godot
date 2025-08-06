using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class PlayerDataTests
    {
        [Test]
        public void Initialization_WhenCreated_SetsDefaultValues()
        {
            // Arrange & Act
            var playerData = new PlayerData(); // 이 클래스는 아직 없습니다.

            // Assert
            Assert.That(playerData.MovementSpeed, Is.EqualTo(300.0f));
            Assert.That(playerData.MaxHealth, Is.EqualTo(100));
            Assert.That(playerData.CurrentHealth, Is.EqualTo(100));
        }
    }
}