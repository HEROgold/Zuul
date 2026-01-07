using System.Collections.Generic;
using System.Linq;

class Room
{
    public string Name { get; }
    private readonly string description;
    private readonly Dictionary<Direction, Exit> exits = new();

    public Room(string name, string desc)
    {
        Name = name;
        description = desc;
    }

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
}
