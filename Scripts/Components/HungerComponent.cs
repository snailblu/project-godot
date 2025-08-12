using System;
using projectgodot.Utils;
using projectgodot.Scripts.Interfaces;
using projectgodot.Constants;

namespace projectgodot
{
    public class HungerComponent : IHunger
    {
        public int MaxHunger { get; private set; }
        public int CurrentHunger { get; private set; }
        public bool IsStarving => CurrentHunger <= 0;

        public event Action<int> HungerChanged;
        public event Action StarvationStarted;
        public event Action<int> StarvationDamageApplied;
        
        // 누적 허기 감소량 (작은 deltaTime 값들을 누적하기 위함)
        private float _accumulatedHungerDecrease = 0f;
        
        // 누적 굶주림 데미지 (작은 deltaTime 값들을 누적하기 위함)
        private float _accumulatedStarvationDamage = 0f;

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
        
        public void ProcessStarvation(float deltaTime)
        {
            if (deltaTime <= 0 || !IsStarving) 
            {
                // 굶주림 상태가 아닐 때는 누적 데미지 초기화
                _accumulatedStarvationDamage = 0f;
                return;
            }
            
            // 누적 방식으로 굶주림 데미지 처리
            _accumulatedStarvationDamage += GameConstants.Hunger.STARVATION_DAMAGE_RATE * deltaTime;
            
            // 누적된 데미지가 1 이상일 때 체력 감소 실행
            if (_accumulatedStarvationDamage >= 1.0f)
            {
                int damageToApply = (int)Math.Floor(_accumulatedStarvationDamage);
                _accumulatedStarvationDamage -= damageToApply;
                
                StarvationDamageApplied?.Invoke(damageToApply);
                GodotLogger.SafePrint($"Starvation damage applied: {damageToApply}");
            }
        }
    }
}