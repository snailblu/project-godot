using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class ScoreManagerTests
    {
        [Test]
        public void AddScore_IncreasesTotalScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();

            // Act
            scoreManager.AddScore(100);

            // Assert
            Assert.That(scoreManager.CurrentScore, Is.EqualTo(100));
        }

        [Test]
        public void AddScore_EmitsScoreChangedEvent()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            int capturedScore = -1;
            bool eventRaised = false;

            scoreManager.ScoreChanged += (score) => {
                capturedScore = score;
                eventRaised = true;
            };

            // Act
            scoreManager.AddScore(150);

            // Assert
            Assert.That(eventRaised, Is.True, "ScoreChanged 이벤트가 발생해야 합니다");
            Assert.That(capturedScore, Is.EqualTo(150), "이벤트로 전달된 점수가 올바르지 않습니다");
        }

        [Test]
        public void AddScore_WithZeroOrNegative_DoesNotChangeScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            scoreManager.AddScore(50); // 초기 점수 설정

            // Act
            scoreManager.AddScore(0);
            scoreManager.AddScore(-10);

            // Assert
            Assert.That(scoreManager.CurrentScore, Is.EqualTo(50), "0이나 음수 점수는 무시되어야 합니다");
        }

        [Test]
        public void AddScore_MultipleValues_AccumulatesCorrectly()
        {
            // Arrange
            var scoreManager = new ScoreManager();

            // Act
            scoreManager.AddScore(100);
            scoreManager.AddScore(50);
            scoreManager.AddScore(25);

            // Assert
            Assert.That(scoreManager.CurrentScore, Is.EqualTo(175));
        }

        [Test]
        public void GetCurrentScore_ReturnsCorrectValue()
        {
            // Arrange
            var scoreManager = new ScoreManager();

            // Act & Assert
            Assert.That(scoreManager.CurrentScore, Is.EqualTo(0), "초기 점수는 0이어야 합니다");
            
            scoreManager.AddScore(75);
            Assert.That(scoreManager.CurrentScore, Is.EqualTo(75), "점수 추가 후 올바른 값을 반환해야 합니다");
        }
    }
}