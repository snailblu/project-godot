using System;
using projectgodot.Helpers;

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
            if (!ValidationHelper.IsPositiveValue(amount)) return;
            
            CurrentScore += amount;
            ScoreChanged?.Invoke(CurrentScore);
        }
    }
}