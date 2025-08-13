public class HealthComponent
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public HealthComponent(int initialHealth)
    {
        MaxHealth = initialHealth;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }
}