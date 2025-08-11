using Godot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class SceneManager : Node
    {
        public override void _Ready()
        {
            GD.Print("SceneManager 초기화 완료");
            
            // 전역 이벤트 구독
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
            {
                events.StartGameRequested += OnStartGameRequested;
                events.ShowMainMenuRequested += OnShowMainMenuRequested;
                events.GameOver += OnGameOver;
            }
        }

        public void ChangeToMainMenu()
        {
            GD.Print("메인 메뉴로 씬 전환");
            GetTree().ChangeSceneToFile(GameConstants.Scenes.MAIN_MENU);
        }

        public void ChangeToGame()
        {
            GD.Print("게임 씬으로 전환");
            GetTree().ChangeSceneToFile(GameConstants.Scenes.GAME);
        }

        public void ChangeToGameOver()
        {
            GD.Print("게임 오버 씬으로 전환");
            
            // GameData에 현재 게임 정보 저장
            var gameData = GetNode<GameData>("/root/GameData");
            var events = EventsHelper.GetEventsNode(this);
            
            // 현재 점수와 웨이브 정보를 GameData에 저장
            // 이 정보들은 GameManager에서 관리되므로 이벤트를 통해 가져와야 함
            
            // 물리 콜백 중 안전한 씬 전환을 위해 call_deferred 사용
            CallDeferred(nameof(DeferredChangeToGameOver));
        }

        private void DeferredChangeToGameOver()
        {
            GetTree().ChangeSceneToFile(GameConstants.Scenes.GAME_OVER);
        }

        // 이벤트 핸들러들
        private void OnStartGameRequested()
        {
            GD.Print("게임 시작 요청 받음");
            ChangeToGame();
        }

        private void OnShowMainMenuRequested()
        {
            GD.Print("메인 메뉴 표시 요청 받음");
            ChangeToMainMenu();
        }

        private void OnGameOver()
        {
            GD.Print("게임 오버 이벤트 받음");
            ChangeToGameOver();
        }

        public override void _ExitTree()
        {
            // 이벤트 연결 해제
            var events = EventsHelper.GetEventsNodeSafe(this);
            if (events != null)
            {
                events.StartGameRequested -= OnStartGameRequested;
                events.ShowMainMenuRequested -= OnShowMainMenuRequested;
                events.GameOver -= OnGameOver;
            }
        }
    }
}