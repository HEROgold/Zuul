public readonly record struct PlayerStats(int Score, int MovesCount)
{
    private readonly EntityStats entityStats;

    public PlayerStats(int maxHealth = 100) : this(default, default) =>
        (entityStats, Score, MovesCount) = (new EntityStats(maxHealth), 0, 0);

    private PlayerStats(EntityStats stats, int score, int moves) : this(default, default) =>
        (entityStats, Score, MovesCount) = (stats, score, moves);

    public PlayerStats Heal(int amount) => new(entityStats.Heal(amount), Score, MovesCount);
    public PlayerStats FullHeal() => new(entityStats.FullHeal(), Score, MovesCount);
    public PlayerStats TakeDamage(int damage) => new(entityStats.TakeDamage(damage), Score, MovesCount);
    public PlayerStats ModifyHealth(int delta) => new(entityStats.ModifyHealth(delta), Score, MovesCount);

    public PlayerStats WithScore(int newScore) => this with { Score = Math.Max(0, newScore) };
    public PlayerStats AddScore(int points) => WithScore(Score + points);
    public PlayerStats IncrementMoves() => this with { MovesCount = MovesCount + 1 };

    public int Health => entityStats.Health;
    public int MaxHealth => entityStats.MaxHealth;
    public bool IsAlive => entityStats.IsAlive;
    public bool IsFullHealth => entityStats.IsFullHealth;
    public double HealthPercentage => entityStats.HealthPercentage;

    public string GetHealthBar(int barLength = 20) => entityStats.GetHealthBar(barLength);

    public string GetStatusDescription() => entityStats.GetStatusDescription() switch
    {
        "Excellent health" => "You are in excellent health.",
        "Good health" => "You are in good health.",
        "Somewhat weak" => "You are feeling somewhat weak.",
        "Badly wounded" => "You are badly wounded.",
        "Near death" => "You are near death!",
        "Dead" => "You are dead.",
        _ => "Unknown status"
    };

    public override string ToString() => $"Health: {Health}/{MaxHealth} | Score: {Score} | Moves: {MovesCount}";
}
