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

        public override void _Ready()
        {
            GD.Print("GameManager 초기화 시작");
            
            // SceneFactory 자식 노드에서 찾기
            _sceneFactory = GetNode<SceneFactory>("SceneFactory");
            
            // 의존성 주입: Spawner를 생성할 때 Factory를 넘겨줌
            _zombieSpawner = new ZombieSpawner(_sceneFactory);
            
            // WaveManager 초기화 (비즈니스 로직)
            _waveManager = new WaveManager();
            
            // GameController 초기화 (테스트된 비즈니스 로직)
            _gameController = new GameController(_waveManager, _zombieSpawner);
            _gameController.SetZombieScene(_zombieScene);
            
            // GameController 이벤트 구독
            _gameController.WaveCleared += OnWaveCleared;
            
            // SceneFactory 좀비 사망 이벤트 구독
            _sceneFactory.ZombieDied += () => _gameController.OnZombieDied();
            
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