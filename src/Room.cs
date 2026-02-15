public class Room(string name, string desc)
{
    public string Name { get; } = name;
    private readonly string description = desc;
    private readonly Dictionary<Direction, Exit> exits = [];
    public Enemy enemy;

    public Inventory Chest { get; } = new(10000);

    public void SetExit(Direction direction, Exit exit) => exits[direction] = exit;

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

    private string GetExitString() => exits.Count == 0 ? "Exits: none"
        : $"Exits: {string.Join(", ", exits.Keys.Select(d => d.ToDirectionString()))}";

    public void AddLock() => IsLocked = true;
    public void Unlock() => IsLocked = false;
    public bool IsLocked { get; private set; }

    public bool IsNurgleLocked { get; private set; }
    public void AddNurgleLock() => IsNurgleLocked = true;

    public void AddEnemy(Enemy enemy) => this.enemy = enemy;

    public string GetDescription(bool includeMap = false, string mapContent = null)
    {
        var desc = GetLongDescription();
        return includeMap && !string.IsNullOrEmpty(mapContent)
            ? $"{desc}\n\n{mapContent}"
            : desc;
    }
}
