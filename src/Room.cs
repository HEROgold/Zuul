using System.Collections.Generic;
using System.Linq;

class Room
{
    private readonly string description;
    private readonly Dictionary<Direction, Exit> exits = new();

    public Room(string desc) => description = desc;

    public void AddExit(Direction direction, Exit exit) => exits[direction] = exit;

    public string GetShortDescription() => description;

    public string GetLongDescription() => $"You are {description}.\n{GetExitString()}";

    public Exit GetExit(Direction direction) => exits.TryGetValue(direction, out Exit exit) ? exit : null;

    public IEnumerable<Direction> GetAvailableExits() => exits.Keys;

    private string GetExitString() => !exits.Any() ? "Exits: none"
        : $"Exits: {string.Join(", ", exits.Keys.Select(d => d.ToDirectionString()))}";
}
