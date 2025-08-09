using Godot;
using projectgodot;

namespace projectgodot
{
    public partial class PauseMenu : Control
    {
        private Button _resumeButton;
        private Button _settingsButton;
        private Button _mainMenuButton;
        private Button _quitButton;
        private PauseMenuLogic _logic;
        private bool _isPaused = false;

        public override void _Ready()
        {
            _logic = new PauseMenuLogic();

            // UI 노드들 참조 설정
            _resumeButton = GetNode<Button>("PauseContainer/ResumeButton");
            _settingsButton = GetNode<Button>("PauseContainer/SettingsButton");
            _mainMenuButton = GetNode<Button>("PauseContainer/MainMenuButton");
            _quitButton = GetNode<Button>("PauseContainer/QuitButton");

            // 버튼 이벤트 연결
            _resumeButton.Pressed += OnResumePressed;
            _settingsButton.Pressed += OnSettingsPressed;
            _mainMenuButton.Pressed += OnMainMenuPressed;
            _quitButton.Pressed += OnQuitPressed;

            // 초기 상태는 숨김
            Visible = false;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_cancel") || Input.IsActionJustPressed("pause"))
            {
                TogglePause();
            }
        }

        private void OnResumePressed()
        {
            _logic.ResumeGame();
            TogglePause();
        }

        private void OnSettingsPressed()
        {
            _logic.ShowSettings();
        }

        private void OnMainMenuPressed()
        {
            _logic.GoToMainMenu();
            TogglePause();
        }

        private void OnQuitPressed()
        {
            _logic.QuitGame();
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            GetTree().Paused = _isPaused;
            Visible = _isPaused;

            var events = GetEventsNode();
            events?.EmitSignal(Events.SignalName.PauseToggled, _isPaused);
        }

        private Events GetEventsNode()
        {
            if (EnvironmentHelper.IsTestEnvironment())
            {
                return null;
            }

            return GetNode<Events>("/root/Events");
        }

        public override void _ExitTree()
        {
            // 이벤트 연결 해제
            if (_resumeButton != null)
                _resumeButton.Pressed -= OnResumePressed;
            if (_settingsButton != null)
                _settingsButton.Pressed -= OnSettingsPressed;
            if (_mainMenuButton != null)
                _mainMenuButton.Pressed -= OnMainMenuPressed;
            if (_quitButton != null)
                _quitButton.Pressed -= OnQuitPressed;
        }
    }

    public class PauseMenuLogic
    {
        public void ResumeGame()
        {
            // 게임 재개 로직은 PauseMenu에서 직접 처리
        }

        public void ShowSettings()
        {
            var events = GetEventsNode();
            events?.EmitSignal(Events.SignalName.ShowSettingsRequested);
        }

        public void GoToMainMenu()
        {
            var events = GetEventsNode();
            events?.EmitSignal(Events.SignalName.ShowMainMenuRequested);
        }

        public void QuitGame()
        {
            var events = GetEventsNode();
            events?.EmitSignal(Events.SignalName.QuitGameRequested);
        }

        private Events GetEventsNode()
        {
            if (EnvironmentHelper.IsTestEnvironment())
            {
                return null;
            }

            var tree = Engine.GetSingleton("SceneTree") as SceneTree;
            return tree?.GetFirstNodeInGroup("events") as Events;
        }
    }
}