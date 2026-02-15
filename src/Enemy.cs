public class Enemy(int maxHealth, string description, int attackDamage)
{
    private EntityStats stats = new EntityStats(maxHealth);
    public int AttackDamage { get; init; } = attackDamage;
    public string Description { get; init; } = description;

    public int MaxHealth => stats.MaxHealth;
    public int CurrentHealth => stats.Health;
    public bool IsAlive => stats.IsAlive;
    public double HealthPercentage => stats.HealthPercentage;

    public void TakeDamage(int damage) => stats = stats.TakeDamage(damage);
    public void Heal(int amount) => stats = stats.Heal(amount);

    public string GetHealthBar(int barLength = 20) => stats.GetHealthBar(barLength);
}
