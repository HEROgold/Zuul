using System.Collections.Generic;
using System.Linq;

class Room
{
    public string Name { get; }
    private readonly string description;
    private readonly Dictionary<Direction, Exit> exits = new();
    private Inventory chest;
    private bool isLocked;
    private bool isNurgleLocked;
    public Enemy enemy;

    public Room(string name, string desc)
    {
        Name = name;
        description = desc;
        chest = new Inventory(10000);
        isLocked = false;
        isNurgleLocked = false;
    }

    public Inventory Chest => chest;

    public void AddExit(Direction direction, Exit exit) => exits[direction] = exit;

    public string GetShortDescription() => description;

    public string GetLongDescription()
    {
        var exitsText = GetExitString();
        return $"You are {description}.\n{exitsText}";
    }

    public Exit GetExit(Direction direction) => exits.TryGetValue(direction, out Exit exit) ? exit : null;

    public IEnumerable<Direction> GetAvailableExits() => exits.Keys;

    public IEnumerable<(Direction direction, Room destination)> GetExitInfos() =>
        exits.Select(kv => (kv.Key, kv.Value.Destination));

    private string GetExitString() => !exits.Any() ? "Exits: none"
        : $"Exits: {string.Join(", ", exits.Keys.Select(d => d.ToDirectionString()))}";

    public void AddLock() => isLocked = true;
    public void RemoveLock() => isLocked = false;
    public bool GetLock() => isLocked;

    public void AddNurgleLock() => isNurgleLocked = true;
    public void RemoveNurgleLock() => isNurgleLocked = false;
    public bool GetNurgleLock() => isNurgleLocked;

    public void AddEnemy(Enemy enemy) => this.enemy = enemy;
}
