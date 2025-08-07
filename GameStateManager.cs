using System;

namespace projectgodot
{
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public class GameStateManager
    {
        public GameState CurrentState { get; private set; }
        
        public event Action<GameState> StateChanged;

        public GameStateManager()
        {
            CurrentState = GameState.MainMenu;
        }

        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return; // 같은 상태로의 전환은 무시
            
            CurrentState = newState;
            StateChanged?.Invoke(CurrentState);
        }

        public bool CanTransitionTo(GameState from, GameState to)
        {
            if (from == to) return false; // 동일한 상태로의 전환은 불가능
            
            // 모든 전환을 허용하는 단순한 구현
            // 나중에 필요하면 더 복잡한 전환 규칙을 추가할 수 있음
            return true;
        }
    }
}