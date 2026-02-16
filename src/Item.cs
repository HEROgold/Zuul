public readonly struct Item
{
    public int Weight { get; init; }
    public string Description { get; init; }

    public Item(int weight, string description) => (Weight, Description) = (weight, description);

    public override string ToString() => $"{Description} ({Weight}kg)";

    public override bool Equals(object obj) =>
        obj is Item other && Description == other.Description;

    public override int GetHashCode() => Description?.GetHashCode() ?? 0;

    public static bool operator ==(Item left, Item right) => left.Equals(right);
    public static bool operator !=(Item left, Item right) => !left.Equals(right);

    public bool Use(Player player, Direction? direction = null) => this switch
    {
        _ when this == ItemProvider.Bandage => UseHealingItem(player, 20, "bandage"),
        _ when this == ItemProvider.Medkit => UseHealingItem(player, 50, "medkit"),
        _ when this == ItemProvider.Key => UseKey(player, direction),
        _ when this == ItemProvider.Hydraulics => UseHydraulics(player, direction),
        _ => UseFallback(player)
    };

    private bool UseHealingItem(Player player, int healAmount, string itemName)
    {
        if (player.Health >= player.MaxHealth - healAmount)
        {
            Console.WriteLine($"You don't need to use the {itemName} right now.");
            player.Backpack.Put(this);
            return false;
        }

        player.Heal(healAmount);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Used {itemName}: +{healAmount}HP. Health: {player.Health}HP");
        Console.ForegroundColor = ConsoleColor.White;
        return true;
    }

    private bool UseKey(Player player, Direction? direction)
    {
        if (!direction.HasValue)
        {
            Console.WriteLine("Use key in which direction?");
            player.Backpack.Put(this);
            return false;
        }

        if (player.CurrentRoom?.GetExit(direction.Value)?.Destination is not Room targetRoom)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No room in that direction.");
            Console.ForegroundColor = ConsoleColor.White;
            player.Backpack.Put(this);
            return false;
        }

        if (!targetRoom.IsLocked)
        {
            Console.WriteLine("That room isn't locked.");
            player.Backpack.Put(this);
            return false;
        }

        targetRoom.Unlock();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The door is now unlocked.");
        Console.ForegroundColor = ConsoleColor.White;
        return true;
    }

    private bool UseHydraulics(Player player, Direction? direction)
    {
        if (!direction.HasValue)
        {
            Console.WriteLine("Use hydraulics in which direction?");
            player.Backpack.Put(this);
            return false;
        }

        if (player.CurrentRoom?.GetExit(direction.Value)?.Destination is not Room targetRoom)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No room in that direction.");
            Console.ForegroundColor = ConsoleColor.White;
            player.Backpack.Put(this);
            return false;
        }

        if (!targetRoom.IsLocked)
        {
            Console.WriteLine("That door doesn't need hydraulics.");
            player.Backpack.Put(this);
            return false;
        }

        targetRoom.Unlock();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("You use the hydraulics to force open the heavy door!");
        Console.ForegroundColor = ConsoleColor.White;
        return true;
    }

    private bool UseFallback(Player player)
    {
        Console.WriteLine($"You can't use {Description} right now.");
        player.Backpack.Put(this);
        return false;
    }
}