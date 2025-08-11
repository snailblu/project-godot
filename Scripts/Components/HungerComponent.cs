using System;
using projectgodot.Utils;

namespace projectgodot
{
    public class HungerComponent
    {
        public int MaxHunger { get; private set; }
        public int CurrentHunger { get; private set; }
        public bool IsStarving => CurrentHunger <= 0;

        public event Action<int> HungerChanged;
        public event Action StarvationStarted;
        
        // 누적 허기 감소량 (작은 deltaTime 값들을 누적하기 위함)
        private float _accumulatedHungerDecrease = 0f;

        public HungerComponent(int maxHunger)
        {
            MaxHunger = maxHunger;
            CurrentHunger = maxHunger;
        }

        public void DecreaseHunger(int amount)
        {
            var wasNotStarving = CurrentHunger > 0;
            CurrentHunger -= amount;
            if (CurrentHunger < 0)
            {
                CurrentHunger = 0;
            }

            HungerChanged?.Invoke(CurrentHunger);
            
            // Check if just became starving
            if (wasNotStarving && CurrentHunger <= 0)
            {
                StarvationStarted?.Invoke();
            }
        }

        public void Eat(int foodValue)
        {
            CurrentHunger += foodValue;
            if (CurrentHunger > MaxHunger)
            {
                CurrentHunger = MaxHunger;
            }
            
            HungerChanged?.Invoke(CurrentHunger);
        }

        public void ProcessHunger(float deltaTime, float hungerDecreaseRate)
        {
            if (deltaTime <= 0) return;
            
            // 누적 방식으로 허기 감소 처리
            _accumulatedHungerDecrease += hungerDecreaseRate * deltaTime;
            
            // 누적된 감소량이 1 이상일 때 허기 감소 실행
            if (_accumulatedHungerDecrease >= 1.0f)
            {
                int hungerToDecrease = (int)Math.Floor(_accumulatedHungerDecrease);
                _accumulatedHungerDecrease -= hungerToDecrease;
                
                DecreaseHunger(hungerToDecrease);
            }
        }
    }
}