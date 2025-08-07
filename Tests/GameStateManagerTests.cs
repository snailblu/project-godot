using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class GameStateManagerTests
    {
        [Test]
        public void ChangeState_UpdatesCurrentState()
        {
            // Arrange
            var gameStateManager = new GameStateManager();

            // Act
            gameStateManager.ChangeState(GameState.Playing);

            // Assert
            Assert.That(gameStateManager.CurrentState, Is.EqualTo(GameState.Playing));
        }

        [Test]
        public void ChangeState_EmitsStateChangedEvent()
        {
            // Arrange
            var gameStateManager = new GameStateManager();
            GameState capturedState = GameState.MainMenu;
            bool eventRaised = false;

            gameStateManager.StateChanged += (newState) => {
                capturedState = newState;
                eventRaised = true;
            };

            // Act
            gameStateManager.ChangeState(GameState.GameOver);

            // Assert
            Assert.That(eventRaised, Is.True, "StateChanged 이벤트가 발생해야 합니다");
            Assert.That(capturedState, Is.EqualTo(GameState.GameOver), "이벤트로 전달된 상태가 올바르지 않습니다");
        }

        [Test]
        public void InitialState_ShouldBeMainMenu()
        {
            // Arrange & Act
            var gameStateManager = new GameStateManager();

            // Assert
            Assert.That(gameStateManager.CurrentState, Is.EqualTo(GameState.MainMenu));
        }

        [Test]
        public void CanTransitionTo_ValidTransitions_ReturnsTrue()
        {
            // Arrange
            var gameStateManager = new GameStateManager();

            // Act & Assert
            Assert.That(gameStateManager.CanTransitionTo(GameState.MainMenu, GameState.Playing), Is.True);
            Assert.That(gameStateManager.CanTransitionTo(GameState.Playing, GameState.GameOver), Is.True);
            Assert.That(gameStateManager.CanTransitionTo(GameState.GameOver, GameState.MainMenu), Is.True);
        }

        [Test]
        public void CanTransitionTo_InvalidTransitions_ReturnsFalse()
        {
            // Arrange
            var gameStateManager = new GameStateManager();

            // Act & Assert
            // 동일한 상태로의 전환은 불필요
            Assert.That(gameStateManager.CanTransitionTo(GameState.Playing, GameState.Playing), Is.False);
        }

        [Test]
        public void ChangeState_WithSameState_DoesNotEmitEvent()
        {
            // Arrange
            var gameStateManager = new GameStateManager();
            gameStateManager.ChangeState(GameState.Playing); // 초기 상태 설정
            
            bool eventRaised = false;
            gameStateManager.StateChanged += (newState) => {
                eventRaised = true;
            };

            // Act
            gameStateManager.ChangeState(GameState.Playing); // 같은 상태로 변경 시도

            // Assert
            Assert.That(eventRaised, Is.False, "동일한 상태로 변경 시 이벤트가 발생하지 않아야 합니다");
        }
    }
}