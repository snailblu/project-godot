using System;

namespace projectgodot
{
    public class ScoreManager
    {
        public int CurrentScore { get; private set; }
        
        public event Action<int> ScoreChanged;

        public ScoreManager()
        {
            CurrentScore = 0;
        }

        public void AddScore(int amount)
        {
            if (amount <= 0) return;
            
            CurrentScore += amount;
            ScoreChanged?.Invoke(CurrentScore);
        }
    }
}