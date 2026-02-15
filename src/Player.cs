public class Player(int initialHealth = 100)
{
    private readonly Random random = new();

    public Room CurrentRoom { get; set; }
    public PlayerStats Stats { get; private set; } = new(initialHealth);
    public Inventory Backpack { get; } = new(100);

    public void TakeDamage(int damage) => Stats = Stats.TakeDamage(damage);
    public void Heal(int amount) => Stats = Stats.Heal(amount);
    public void FullHeal() => Stats = Stats.FullHeal();
    public void AddScore(int points) => Stats = Stats.AddScore(points);
    public void IncrementMoves() => Stats = Stats.IncrementMoves();
    public void Damage() => TakeDamage(10);
    public void Kill() => Stats = Stats.TakeDamage(MaxHealth);

    public void HealAmount(int amount)
    {
        if (Health > MaxHealth - amount)
        {
            Console.WriteLine("You aren't all that injured are you?");
            return;
        }

        Heal(amount);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"You healed! Your health is now: {Health}HP");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void DamageAmount(int amount)
    {
        if (Health < amount)
        {
            Console.WriteLine("That would kill u.");
            return;
        }

        TakeDamage(amount);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"You took {amount} damage! Your health is now: {Health}HP");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public bool IsAlive => Stats.IsAlive;
    public int Health => Stats.Health;
    public int MaxHealth => Stats.MaxHealth;
    public int Score => Stats.Score;
    public int MovesCount => Stats.MovesCount;

    public void ShowLowHealthWarning()
    {
        var message = Stats.HealthPercentage switch
        {
            <= 0.2 and > 0 => "You feel miserable. You should heal!",
            <= 0.4 => "You feel hurt.",
            _ => null
        };

        if (message != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public void ShowStatus()
    {
        Console.WriteLine("\n=== Player Status ===");
        Console.WriteLine(Stats.GetHealthBar());
        Console.WriteLine(Stats.GetStatusDescription());
        Console.WriteLine($"Score: {Score}");
        Console.WriteLine($"Moves: {MovesCount}");
        Console.WriteLine();
        Backpack.Display();
    }

    public void Attack(Enemy enemy)
    {
        if (enemy == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is nothing here to attack.");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        int playerAttack = random.Next(1, 11) == 1 ? 0 : random.Next(15, 26);
        if (playerAttack > 0) enemy.TakeDamage(playerAttack);

        bool dodged = random.Next(1, 11) == 1;
        int enemyDamage = dodged ? 0 : random.Next(5, 15);
        if (enemyDamage > 0) TakeDamage(enemyDamage);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"You got hit for {enemyDamage} damage. You got {Health}HP left");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(enemy.IsAlive
            ? $"You hit the enemy for {playerAttack} damage. It still has {enemy.CurrentHealth}HP remaining"
            : "You slayed the thing.");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public bool UseItem(string itemName, Direction? direction = null)
    {
        if (Backpack.Get(itemName) is not Item item)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You don't have '{itemName}'.");
            Console.ForegroundColor = ConsoleColor.White;
            return false;
        }

        return item.Use(this, direction);
    }
}
