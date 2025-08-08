using System;
using Godot;

namespace projectgodot
{
    public class HealthComponent
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

        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;
            
            var previousHealth = CurrentHealth;
            CurrentHealth -= amount;
            
            // 데미지 로그 출력 (테스트 환경에서는 스킵)
            if (System.Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") != "true")
            {
                try
                {
                    GD.Print($"Zombie took {amount} damage! Current health: {CurrentHealth}/{MaxHealth}");
                }
                catch
                {
                    // Godot 환경이 아닐 때는 무시
                }
            }
            
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            HealthChanged?.Invoke(CurrentHealth);

            // 이전 체력이 0보다 크고, 현재 체력이 0 이하가 되었을 때 (즉, 방금 죽었을 때)
            if (previousHealth > 0 && CurrentHealth <= 0)
            {
                if (System.Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") != "true")
                {
                    try
                    {
                        GD.Print("Zombie DIED!");
                    }
                    catch
                    {
                        // Godot 환경이 아닐 때는 무시
                    }
                }
                Died?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (amount <= 0) return;
            
            CurrentHealth += amount;
            
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            HealthChanged?.Invoke(CurrentHealth);
        }
    }
}