using System.Collections.Generic;
using System.Linq;

class Room
{
    private readonly string description;
    private readonly Dictionary<Direction, Room> exits = new();
    private readonly Dictionary<Direction, (int minDamage, int maxDamage)> hazardousExits = new();

    public Room(string desc) => description = desc;

    public void AddExit(Direction direction, Room neighbor) => exits[direction] = neighbor;

    public void AddHazardousExit(Direction direction, Room neighbor, int minDamage, int maxDamage)
    {
        exits[direction] = neighbor;
        hazardousExits[direction] = (minDamage, maxDamage);
    }

    public void AddExit(string directionString, Room neighbor)
    {
        var direction = DirectionExtensions.FromString(directionString);
        if (direction.HasValue) AddExit(direction.Value, neighbor);
    }

    public void AddHazardousExit(string directionString, Room neighbor, int minDamage, int maxDamage)
    {
        var direction = DirectionExtensions.FromString(directionString);
        if (direction.HasValue) AddHazardousExit(direction.Value, neighbor, minDamage, maxDamage);
    }

    public bool IsExitHazardous(Direction direction) => hazardousExits.ContainsKey(direction);

    public (int minDamage, int maxDamage) GetExitDamage(Direction direction) =>
        hazardousExits.TryGetValue(direction, out var damage) ? damage : (0, 0);

    public string GetShortDescription() => description;

    public string GetLongDescription() => $"You are {description}.\n{GetExitString()}";

    public Room GetExit(Direction direction) => exits.TryGetValue(direction, out Room room) ? room : null;

    public Room GetExit(string directionString)
    {
        var direction = DirectionExtensions.FromString(directionString);
        return direction.HasValue ? GetExit(direction.Value) : null;
    }

    public IEnumerable<Direction> GetAvailableExits() => exits.Keys;

    private string GetExitString() => !exits.Any() ? "Exits: none"
        : $"Exits: {string.Join(", ", exits.Keys.Select(d => d.ToDirectionString()))}";
}
