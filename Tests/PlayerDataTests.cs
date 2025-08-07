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
            var playerData = new PlayerData();

            // Assert
            Assert.That(playerData.MovementSpeed, Is.EqualTo(8000.0f));
        }
    }
}