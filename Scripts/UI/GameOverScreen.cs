using Godot;
using projectgodot;

namespace projectgodot
{
    public partial class GameOverScreen : Control
    {
        private Label _finalScoreLabel;
        private Label _finalWaveLabel;
        private Button _restartButton;
        private Button _mainMenuButton;
        private Button _quitButton;
        private GameOverScreenLogic _logic;

        public override void _Ready()
        {
            _logic = new GameOverScreenLogic();

            // UI 노드들 참조 설정
            _finalScoreLabel = GetNode<Label>("GameOverContainer/FinalScoreLabel");
            _finalWaveLabel = GetNode<Label>("GameOverContainer/FinalWaveLabel");
            _restartButton = GetNode<Button>("GameOverContainer/RestartButton");
            _mainMenuButton = GetNode<Button>("GameOverContainer/MainMenuButton");
            _quitButton = GetNode<Button>("GameOverContainer/QuitButton");

            // 버튼 이벤트 연결
            _restartButton.Pressed += OnRestartPressed;
            _mainMenuButton.Pressed += OnMainMenuPressed;
            _quitButton.Pressed += OnQuitPressed;

            // GameData에서 최종 점수와 웨이브 정보 가져와서 표시
            var gameData = GetNode<GameData>("/root/GameData");
            var (finalScore, finalWave) = gameData.GetGameOverData();
            ShowGameOverScreen(finalScore, finalWave);
        }

        private void OnRestartPressed()
        {
            _logic.RestartGame(); // 비즈니스 로직 호출 (현재는 빈 메서드)
            
            // 게임 재시작 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.StartGameRequested);
            
            Hide();
        }

        private void OnMainMenuPressed()
        {
            _logic.GoToMainMenu(); // 비즈니스 로직 호출 (현재는 빈 메서드)
            
            // 메인 메뉴 표시 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.ShowMainMenuRequested);
            
            Hide();
        }

        private void OnQuitPressed()
        {
            _logic.QuitGame(); // 비즈니스 로직 호출 (현재는 빈 메서드)
            
            // 게임 종료 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.QuitGameRequested);
        }

        public void ShowGameOverScreen(int finalScore, int finalWave)
        {
            if (_finalScoreLabel != null)
                _finalScoreLabel.Text = _logic.FormatFinalScore(finalScore);
            
            if (_finalWaveLabel != null)
                _finalWaveLabel.Text = _logic.FormatFinalWave(finalWave);
            
            Show();
        }

        public override void _ExitTree()
        {
            // 이벤트 연결 해제
            if (_restartButton != null)
                _restartButton.Pressed -= OnRestartPressed;
            if (_mainMenuButton != null)
                _mainMenuButton.Pressed -= OnMainMenuPressed;
            if (_quitButton != null)
                _quitButton.Pressed -= OnQuitPressed;
        }
    }

    public class GameOverScreenLogic
    {
        // 순수 C# 클래스 - Godot API 접근 없음
        // UI 로직은 GameOverScreen 클래스에서 처리
        
        public void RestartGame()
        {
            // 게임 재시작 관련 비즈니스 로직이 필요한 경우 여기에 추가
        }

        public void GoToMainMenu()
        {
            // 메인 메뉴 이동 관련 비즈니스 로직이 필요한 경우 여기에 추가
        }

        public void QuitGame()
        {
            // 게임 종료 관련 비즈니스 로직이 필요한 경우 여기에 추가
        }

        public string FormatFinalScore(int score)
        {
            return $"최종 점수: {score}";
        }

        public string FormatFinalWave(int wave)
        {
            return $"도달 웨이브: {wave}";
        }
    }
}