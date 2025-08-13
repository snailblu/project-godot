using System;
using Godot;
using projectgodot.Helpers;
using projectgodot.Utils;
using projectgodot.Scripts.Interfaces;
using projectgodot.Components;

namespace projectgodot
{
    public class HealthComponent : IHealth, IGameComponent
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;

        public event Action Died;
        public event Action<int> HealthChanged;
        

        public HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void Initialize(Player player)
        {
            // 허기 시스템 제거로 더 이상 초기화할 것이 없음
        }

        public void TakeDamage(int amount)
        {
            if (!ValidationHelper.IsPositiveValue(amount)) return;
            
            var previousHealth = CurrentHealth;
            CurrentHealth -= amount;
            
            GodotLogger.LogDamage(amount, CurrentHealth, MaxHealth);
            
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            HealthChanged?.Invoke(CurrentHealth);

            // 이전 체력이 0보다 크고, 현재 체력이 0 이하가 되었을 때 (즉, 방금 죽었을 때)
            if (previousHealth > 0 && CurrentHealth <= 0)
            {
                GodotLogger.LogDeath();
                Died?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (!ValidationHelper.IsPositiveValue(amount)) return;
            
            CurrentHealth += amount;
            
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            HealthChanged?.Invoke(CurrentHealth);
        }
    }
}