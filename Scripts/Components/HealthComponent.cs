public class HealthComponent
{
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public HealthComponent(int initialHealth)
    {
        CurrentHealth = initialHealth;
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