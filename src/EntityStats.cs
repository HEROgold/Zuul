public readonly record struct EntityStats(int Health, int MaxHealth)
{
    public EntityStats(int maxHealth) : this(default, default) =>
        (Health, MaxHealth) = (maxHealth, maxHealth);

    private EntityStats WithHealth(int newHealth) =>
        this with { Health = Math.Clamp(newHealth, 0, MaxHealth) };

    public EntityStats ModifyHealth(int delta) => WithHealth(Health + delta);
    public EntityStats Heal(int amount) => WithHealth(Health + amount);
    public EntityStats FullHeal() => WithHealth(MaxHealth);
    public EntityStats TakeDamage(int damage) => WithHealth(Health - damage);

    public bool IsAlive => Health > 0;
    public bool IsFullHealth => Health >= MaxHealth;
    public double HealthPercentage => MaxHealth > 0 ? (double)Health / MaxHealth : 0;

    public string GetHealthBar(int barLength = 20)
    {
        int filledLength = (int)(HealthPercentage * barLength);
        string filled = new('█', filledLength);
        string empty = new('░', barLength - filledLength);
        return $"[{filled}{empty}] {Health}/{MaxHealth}";
    }

    public string GetStatusDescription() => HealthPercentage switch
    {
        >= 0.8 => "Excellent health",
        >= 0.6 => "Good health",
        >= 0.4 => "Somewhat weak",
        >= 0.2 => "Badly wounded",
        > 0 => "Near death",
        _ => "Dead"
    };
}
