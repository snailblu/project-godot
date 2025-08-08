using Godot;

namespace projectgodot
{
    // Godot AutoLoad로 등록될 전역 싱글톤 이벤트 버스
    public partial class Events : Node
    {
        // 좀비가 죽었을 때 발생할 시그널. 점수 정보를 함께 전달.
        [Signal]
        public delegate void ZombieDiedEventHandler(int scoreValue);

        // 플레이어 체력이 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void PlayerHealthChangedEventHandler(int currentHealth, int maxHealth);
        
        // 웨이브 정보가 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void WaveChangedEventHandler(int waveNumber);

        // 점수가 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void ScoreChangedEventHandler(int newScore);

        // 게임 상태가 변경되었을 때 발생할 시그널
        [Signal]
        public delegate void GameStateChangedEventHandler(int newState);

        public override void _Ready()
        {
            GD.Print("Events 이벤트 버스가 초기화되었습니다.");
        }
    }
}