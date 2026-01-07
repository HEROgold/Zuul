using System;

public readonly struct PlayerStats
{
    public int Health { get; init; }
    public int MaxHealth { get; init; }
    public int Score { get; init; }
    public int MovesCount { get; init; }

    public PlayerStats(int maxHealth = 100) =>
        (Health, MaxHealth, Score, MovesCount) = (maxHealth, maxHealth, 0, 0);

    public PlayerStats WithHealth(int newHealth) =>
        this with { Health = Math.Clamp(newHealth, 0, MaxHealth) };

    public PlayerStats ModifyHealth(int delta) => WithHealth(Health + delta);
    public PlayerStats Heal(int amount) => WithHealth(Health + amount);
    public PlayerStats FullHeal() => WithHealth(MaxHealth);
    public PlayerStats TakeDamage(int damage) => WithHealth(Health - damage);
    public PlayerStats WithScore(int newScore) => this with { Score = Math.Max(0, newScore) };
    public PlayerStats AddScore(int points) => WithScore(Score + points);
    public PlayerStats IncrementMoves() => this with { MovesCount = MovesCount + 1 };

    public bool IsAlive => Health > 0;
    public bool IsFullHealth => Health >= MaxHealth;
    public double HealthPercentage => MaxHealth > 0 ? (double)Health / MaxHealth : 0;

    public string GetHealthBar(int barLength = 20)
    {
        int filledLength = (int)(HealthPercentage * barLength);
        string filled = new string('█', filledLength);
        string empty = new string('░', barLength - filledLength);
        return $"[{filled}{empty}] {Health}/{MaxHealth}";
    }

    public string GetStatusDescription() => HealthPercentage switch
    {
        >= 0.8 => "You are in excellent health.",
        >= 0.6 => "You are in good health.",
        >= 0.4 => "You are feeling somewhat weak.",
        >= 0.2 => "You are badly wounded.",
        > 0 => "You are near death!",
        _ => "You are dead."
    };

    public override string ToString() => $"Health: {Health}/{MaxHealth} | Score: {Score} | Moves: {MovesCount}";
}
