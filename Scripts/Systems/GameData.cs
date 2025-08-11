using Godot;

namespace projectgodot
{
    /// <summary>
    /// 씬 간 데이터 전달을 담당하는 AutoLoad 싱글톤
    /// </summary>
    public partial class GameData : Node
    {
        // 게임 데이터
        public int CurrentScore { get; set; } = 0;
        public int CurrentWave { get; set; } = 1;
        public int PlayerHealth { get; set; } = 100;
        public int MaxPlayerHealth { get; set; } = 100;
        
        public override void _Ready()
        {
            GD.Print("GameData 초기화 완료");
            
            // Events 시스템과 연결하여 데이터 자동 업데이트
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
            {
                events.ScoreChanged += OnScoreChanged;
                events.WaveChanged += OnWaveChanged;
                events.PlayerHealthChanged += OnPlayerHealthChanged;
            }
        }
        
        public void ResetGameData()
        {
            CurrentScore = 0;
            CurrentWave = 1;
            PlayerHealth = MaxPlayerHealth;
            GD.Print("게임 데이터 초기화");
        }
        
        // 게임 시작 시 호출
        public void StartNewGame()
        {
            ResetGameData();
            GD.Print("새 게임 시작 - 데이터 초기화됨");
        }
        
        // 게임 오버 정보 가져오기
        public (int finalScore, int finalWave) GetGameOverData()
        {
            return (CurrentScore, CurrentWave);
        }
        
        // 이벤트 핸들러들
        private void OnScoreChanged(int newScore)
        {
            CurrentScore = newScore;
        }
        
        private void OnWaveChanged(int waveNumber)
        {
            CurrentWave = waveNumber;
        }
        
        private void OnPlayerHealthChanged(int currentHealth, int maxHealth)
        {
            PlayerHealth = currentHealth;
            MaxPlayerHealth = maxHealth;
        }
        
        public override void _ExitTree()
        {
            // 이벤트 연결 해제
            var events = EventsHelper.GetEventsNodeSafe(this);
            if (events != null)
            {
                events.ScoreChanged -= OnScoreChanged;
                events.WaveChanged -= OnWaveChanged;
                events.PlayerHealthChanged -= OnPlayerHealthChanged;
            }
        }
    }
}