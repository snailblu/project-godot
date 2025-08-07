using Godot;
using System.Linq;

namespace projectgodot
{
    /// <summary>
    /// 게임의 전체적인 흐름과 상태를 조율하는 매니저 클래스
    /// </summary>
    public partial class GameManager : Node
    {
        [Export] private PackedScene _zombieScene;
        [Export] private Node _spawnLocationsContainer; // 스폰 위치들을 담을 Node
        
        private GameController _gameController;
        private WaveManager _waveManager;
        private ZombieSpawner _zombieSpawner;
        private SceneFactory _sceneFactory;
        private ScoreManager _scoreManager;
        private GameStateManager _gameStateManager;

        public override void _Ready()
        {
            GD.Print("GameManager 초기화 시작");
            
            // SceneFactory 자식 노드에서 찾기
            _sceneFactory = GetNode<SceneFactory>("SceneFactory");
            
            // 의존성 주입: Spawner를 생성할 때 Factory를 넘겨줌
            _zombieSpawner = new ZombieSpawner(_sceneFactory);
            
            // WaveManager 초기화 (비즈니스 로직)
            _waveManager = new WaveManager();
            
            // 점수 매니저 초기화
            _scoreManager = new ScoreManager();
            
            // 게임 상태 매니저 초기화
            _gameStateManager = new GameStateManager();
            
            // GameController 초기화 (테스트된 비즈니스 로직)
            _gameController = new GameController(_waveManager, _zombieSpawner);
            _gameController.SetZombieScene(_zombieScene);
            
            // GameController 이벤트 구독
            _gameController.WaveCleared += OnWaveCleared;
            
            // SceneFactory 좀비 사망 이벤트 구독
            _sceneFactory.ZombieDied += () => _gameController.OnZombieDied();
            
            // WaveManager 이벤트 구독하여 전역 이벤트 버스로 전달
            _waveManager.WaveStarted += OnWaveStarted;
            
            // ScoreManager 이벤트 구독하여 전역 이벤트 버스로 전달
            _scoreManager.ScoreChanged += OnScoreChanged;
            
            // 전역 이벤트 버스 구독
            var events = GetNode<Events>("/root/Events");
            events.ZombieDied += OnZombieDied;
            
            // 게임 시작 후 3초 뒤에 첫 웨이브 시작
            var timer = GetTree().CreateTimer(3.0f);
            timer.Timeout += () => {
                GD.Print("첫 번째 웨이브를 시작합니다!");
                StartNextWave();
            };
        }

        private void StartNextWave()
        {
            // 스폰 위치들 가져오기
            var spawnLocations = _spawnLocationsContainer?.GetChildren().Cast<Marker2D>().ToList();
            
            if (spawnLocations == null || spawnLocations.Count == 0)
            {
                GD.PrintErr("스폰 위치를 찾을 수 없습니다!");
                return;
            }

            var spawnPositions = spawnLocations.Select(loc => loc.GlobalPosition).ToList();
            _gameController.StartWave(spawnPositions);
            
            GD.Print($"웨이브 {_waveManager.CurrentWaveNumber} 시작! {_gameController.ZombiesRemainingInWave}마리 좀비 스폰");
        }

        private void OnWaveCleared()
        {
            GD.Print("모든 좀비 처치 완료! 웨이브 클리어!");
            
            // 5초 후에 다음 웨이브 시작
            var timer = GetTree().CreateTimer(5.0f);
            timer.Timeout += () => {
                GD.Print($"다음 웨이브 준비 중... ({_waveManager.CurrentWaveNumber + 1}번째 웨이브)");
                StartNextWave();
            };
        }

        // 이벤트 핸들러들
        private void OnWaveStarted(int waveNumber, int zombiesToSpawn)
        {
            // WaveManager의 이벤트를 전역 이벤트 버스로 전달
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.WaveChanged, waveNumber);
            
            GD.Print($"웨이브 {waveNumber} 시작 이벤트 발생");
        }

        private void OnScoreChanged(int newScore)
        {
            // ScoreManager의 이벤트를 전역 이벤트 버스로 전달
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.ScoreChanged, newScore);
            
            GD.Print($"점수 변경: {newScore}");
        }

        private void OnZombieDied(int scoreValue)
        {
            // 좀비가 죽으면 점수 추가
            _scoreManager.AddScore(scoreValue);
            GD.Print($"좀비 처치! +{scoreValue}점");
        }

        // 디버그용: 수동으로 웨이브 시작
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {
                if (keyEvent.Keycode == Key.Space && !_waveManager.IsWaveActive)
                {
                    GD.Print("수동 웨이브 시작 (스페이스바)");
                    StartNextWave();
                }
            }
        }
    }
}