// HealthLogic.cs (순수 C# 클래스 - Godot 의존성 없음)
using System;

public class HealthLogic
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    // C#의 표준 이벤트 시스템. Signal을 대체한다.
    public event Action<float, float> HealthChanged;
    public event Action Died;

    public HealthLogic(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0) return;

        CurrentHealth -= amount;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        // 구독자들에게 이벤트(방송)를 보낸다.
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (IsDead)
        {
            Died?.Invoke();
        }
    }
}