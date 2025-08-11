using Godot;
using projectgodot;

namespace projectgodot
{
    public partial class MainMenu : Control
    {
        private Button _startButton;
        private Button _settingsButton; 
        private Button _quitButton;
        private MainMenuLogic _logic;

        public override void _Ready()
        {
            _logic = new MainMenuLogic();

            // 버튼 노드들 참조 설정
            _startButton = GetNode<Button>("MenuContainer/StartButton");
            _settingsButton = GetNode<Button>("MenuContainer/SettingsButton");
            _quitButton = GetNode<Button>("MenuContainer/QuitButton");

            // 버튼 이벤트 연결
            _startButton.Pressed += OnStartPressed;
            _settingsButton.Pressed += OnSettingsPressed;
            _quitButton.Pressed += OnQuitPressed;
        }

        private void OnStartPressed()
        {
            _logic.StartGame(); // 비즈니스 로직 호출 (현재는 빈 메서드)
            
            // GameData 초기화
            var gameData = GetNode<GameData>("/root/GameData");
            gameData.StartNewGame();
            
            // 게임 시작 이벤트 발생
            EventsHelper.EmitSignalSafe(this, Events.SignalName.StartGameRequested);
        }

        private void OnSettingsPressed()
        {
            _logic.ShowSettings(); // 비즈니스 로직 호출 (현재는 빈 메서드)
            
            // 설정 화면 표시 이벤트 발생
            EventsHelper.EmitSignalSafe(this, Events.SignalName.ShowSettingsRequested);
        }

        private void OnQuitPressed()
        {
            _logic.QuitGame(); // 비즈니스 로직 호출 (현재는 빈 메서드)
            
            // 게임 종료 이벤트 발생
            EventsHelper.EmitSignalSafe(this, Events.SignalName.QuitGameRequested);
        }

        public override void _ExitTree()
        {
            // 이벤트 연결 해제
            if (_startButton != null)
                _startButton.Pressed -= OnStartPressed;
            if (_settingsButton != null)
                _settingsButton.Pressed -= OnSettingsPressed;
            if (_quitButton != null)
                _quitButton.Pressed -= OnQuitPressed;
        }
    }

    public class MainMenuLogic
    {
        // 순수 C# 클래스 - Godot API 접근 없음
        // UI 로직은 MainMenu 클래스에서 처리
        
        public void StartGame()
        {
            // 비즈니스 로직이 필요한 경우 여기에 추가
            // 현재는 단순 이벤트 발생이므로 로직 없음
        }

        public void ShowSettings()
        {
            // 설정 관련 비즈니스 로직이 필요한 경우 여기에 추가
        }

        public void QuitGame()
        {
            // 게임 종료 관련 비즈니스 로직이 필요한 경우 여기에 추가
        }
    }

}