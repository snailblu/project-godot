using NUnit.Framework;
using System;
using System.Threading.Tasks;
using projectgodot;
using projectgodot.Constants;

namespace Tests
{
    [TestFixture]
    public class SceneManagerTests
    {
        private SceneManager _sceneManager;

        [SetUp]
        public void SetUp()
        {
            // 테스트 환경에서는 Godot 노드 의존성 없이 테스트
            Environment.SetEnvironmentVariable("TEST_ENVIRONMENT", "true");
            _sceneManager = new SceneManager();
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("TEST_ENVIRONMENT", null);
        }

        [Test]
        public void GameConstants_Scenes_ShouldHaveCorrectPaths()
        {
            // Arrange & Act & Assert
            Assert.That(GameConstants.Scenes.MAIN_MENU, Is.EqualTo("res://Scenes/UI/MainMenu.tscn"));
            Assert.That(GameConstants.Scenes.GAME, Is.EqualTo("res://Scenes/Main/Game.tscn"));
            Assert.That(GameConstants.Scenes.GAME_OVER, Is.EqualTo("res://Scenes/UI/GameOverScreen.tscn"));
        }

        [Test]
        public void SceneManager_ShouldInitializeWithoutError()
        {
            // Arrange & Act & Assert
            Assert.That(_sceneManager, Is.Not.Null);
        }

        // 실제 씬 전환은 Godot 환경이 필요하므로 테스트에서는 검증하지 않음
        // 대신 씬 경로 상수들이 올바른지만 확인
    }
}