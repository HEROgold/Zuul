using System.Runtime.InteropServices;

class Player
{
    private PlayerStats stats;

    public Room CurrentRoom { get; set; }
    public PlayerStats Stats => stats;

    public Player(int initialHealth = 100)
    {
        CurrentRoom = null;
        stats = new PlayerStats(initialHealth);
    }

    public void TakeDamage(int damage) => stats = stats.TakeDamage(damage);
    public void Heal(int amount) => stats = stats.Heal(amount);
    public void FullHeal() => stats = stats.FullHeal();
    public void AddScore(int points) => stats = stats.AddScore(points);
    public void IncrementMoveCounter() => stats = stats.IncrementMoves();

    public bool IsAlive => stats.IsAlive;
    public int Health => stats.Health;
    public int MaxHealth => stats.MaxHealth;
    public int Score => stats.Score;
    public int MovesCount => stats.MovesCount;

    public string GetHealthDisplay() => stats.GetHealthBar();
    public string GetStatusDescription() => stats.GetStatusDescription();
    public string GetFullStatus() => $"{stats}\n{GetStatusDescription()}";
}
