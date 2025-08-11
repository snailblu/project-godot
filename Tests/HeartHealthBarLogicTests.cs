using NUnit.Framework;
using projectgodot;

namespace projectgodot.Tests
{
    [TestFixture]
    public class HeartHealthBarLogicTests
    {
        private HeartHealthBarLogic _heartLogic;

        [SetUp]
        public void SetUp()
        {
            _heartLogic = new HeartHealthBarLogic();
        }

        [Test]
        public void CalculateHeartStates_FullHealth_ReturnsThreeFullHearts()
        {
            // Arrange & Act
            var hearts = _heartLogic.CalculateHeartStates(100, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Full));
        }

        [Test]
        public void CalculateHeartStates_ZeroHealth_ReturnsThreeEmptyHearts()
        {
            // Arrange & Act
            var hearts = _heartLogic.CalculateHeartStates(0, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_Health67_ReturnsTwoFullOneEmpty()
        {
            // Arrange & Act (67 = 4.0 하트 = 2개 가득 + 1개 빈, 100/6=16.67이므로 67/16.67=4.0)
            var hearts = _heartLogic.CalculateHeartStates(67, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_Health50_ReturnsOneFullOneHalfOneEmpty()
        {
            // Arrange & Act (50 = 3.0 하트 = 1개 가득 + 1개 반 + 1개 빈, 50/16.67=3.0)
            var hearts = _heartLogic.CalculateHeartStates(50, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Half));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_Health34_ReturnsOneFullTwoEmpty()
        {
            // Arrange & Act (34 = 2.0 하트 = 1개 가득 + 2개 빈, 34/16.67=2.0)
            var hearts = _heartLogic.CalculateHeartStates(34, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_Health17_ReturnsOneHalfTwoEmpty()
        {
            // Arrange & Act (17 = 1.0 하트 = 1개 반 + 2개 빈, 17/16.67=1.0)
            var hearts = _heartLogic.CalculateHeartStates(17, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Half));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_NegativeHealth_ReturnsThreeEmptyHearts()
        {
            // Arrange & Act
            var hearts = _heartLogic.CalculateHeartStates(-10, 100);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_ZeroMaxHealth_ReturnsThreeEmptyHearts()
        {
            // Arrange & Act
            var hearts = _heartLogic.CalculateHeartStates(50, 0);

            // Assert
            Assert.That(hearts.Length, Is.EqualTo(3));
            Assert.That(hearts[0], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void GetHeartStateText_AllStates_ReturnsCorrectText()
        {
            // Assert
            Assert.That(_heartLogic.GetHeartStateText(HeartState.Full), Is.EqualTo("Full Heart"));
            Assert.That(_heartLogic.GetHeartStateText(HeartState.Half), Is.EqualTo("Half Heart"));
            Assert.That(_heartLogic.GetHeartStateText(HeartState.Empty), Is.EqualTo("Empty Heart"));
        }

        [Test]
        public void CalculateHeartStates_DifferentMaxHealth_WorksCorrectly()
        {
            // Test with maxHealth = 60 (각 반하트당 10씩, 30 = 3.0 하트 = 1개 가득 + 1개 반 + 1개 빈)
            var hearts = _heartLogic.CalculateHeartStates(30, 60); // 3.0 hearts

            Assert.That(hearts[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Half));  
            Assert.That(hearts[2], Is.EqualTo(HeartState.Empty));
        }

        [Test]
        public void CalculateHeartStates_SequentialDecrease_Health84_TwoFullOneHalf()
        {
            // Arrange & Act (84 = 5.0 하트 = 2개 가득 + 1개 반 + 0개 빈)
            var hearts = _heartLogic.CalculateHeartStates(84, 100);

            // Assert - 세 번째 하트가 반으로 줄어든 상태
            Assert.That(hearts[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[1], Is.EqualTo(HeartState.Full));
            Assert.That(hearts[2], Is.EqualTo(HeartState.Half));
        }

        [Test]  
        public void CalculateHeartStates_SequentialDecrease_EdgeCases()
        {
            // 각 경계값들 테스트
            
            // 100 -> 3.0개 하트 (모든 하트 가득)
            var hearts100 = _heartLogic.CalculateHeartStates(100, 100);
            Assert.That(hearts100[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts100[1], Is.EqualTo(HeartState.Full));
            Assert.That(hearts100[2], Is.EqualTo(HeartState.Full));

            // 84 -> 2.5개 하트 (2개 가득, 1개 반) - 100/6*5 = 83.33이므로 84 사용  
            var hearts84 = _heartLogic.CalculateHeartStates(84, 100);
            Assert.That(hearts84[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts84[1], Is.EqualTo(HeartState.Full));
            Assert.That(hearts84[2], Is.EqualTo(HeartState.Half));

            // 67 -> 2.0개 하트 (2개 가득, 1개 빈) - 100/6*4 = 66.67이므로 67 사용
            var hearts67 = _heartLogic.CalculateHeartStates(67, 100);
            Assert.That(hearts67[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts67[1], Is.EqualTo(HeartState.Full));
            Assert.That(hearts67[2], Is.EqualTo(HeartState.Empty));

            // 34 -> 1.0개 하트 (1개 가득, 2개 빈) - 100/6*2 = 33.33이므로 34 사용
            var hearts34 = _heartLogic.CalculateHeartStates(34, 100);
            Assert.That(hearts34[0], Is.EqualTo(HeartState.Full));
            Assert.That(hearts34[1], Is.EqualTo(HeartState.Empty));
            Assert.That(hearts34[2], Is.EqualTo(HeartState.Empty));
        }
    }
}