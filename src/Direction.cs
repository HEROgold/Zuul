using System;
using System.Collections.Generic;
using System.Linq;

// Enum for all valid directions
public enum Direction
{
    North,
    South,
    East,
    West,
    Up,
    Down,
    Northeast,
    Northwest,
    Southeast,
    Southwest
}

// Extension methods for Direction
public static class DirectionExtensions
{
    private static readonly Dictionary<string, Direction> stringToDirectionMap = new()
    {
        { "north", Direction.North },
        { "n", Direction.North },
        { "south", Direction.South },
        { "s", Direction.South },
        { "east", Direction.East },
        { "e", Direction.East },
        { "west", Direction.West },
        { "w", Direction.West },
        { "up", Direction.Up },
        { "u", Direction.Up },
        { "down", Direction.Down },
        { "d", Direction.Down },
        { "northeast", Direction.Northeast },
        { "ne", Direction.Northeast },
        { "northwest", Direction.Northwest },
        { "nw", Direction.Northwest },
        { "southeast", Direction.Southeast },
        { "se", Direction.Southeast },
        { "southwest", Direction.Southwest },
        { "sw", Direction.Southwest }
    };

    private static readonly Dictionary<Direction, string> directionToStringMap = new()
    {
        { Direction.North, "north" },
        { Direction.South, "south" },
        { Direction.East, "east" },
        { Direction.West, "west" },
        { Direction.Up, "up" },
        { Direction.Down, "down" },
        { Direction.Northeast, "northeast" },
        { Direction.Northwest, "northwest" },
        { Direction.Southeast, "southeast" },
        { Direction.Southwest, "southwest" }
    };

    // Convert string to Direction (returns null if invalid)
    public static Direction? FromString(string directionString)
    {
        if (string.IsNullOrWhiteSpace(directionString))
            return null;

        return stringToDirectionMap.TryGetValue(directionString.ToLower().Trim(), out var direction)
            ? direction
            : null;
    }

    // Convert Direction to string
    public static string ToDirectionString(this Direction direction)
    {
        return directionToStringMap.TryGetValue(direction, out var str) ? str : "unknown";
    }

    // Get opposite direction
    public static Direction GetOpposite(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Northeast => Direction.Southwest,
            Direction.Southwest => Direction.Northeast,
            Direction.Northwest => Direction.Southeast,
            Direction.Southeast => Direction.Northwest,
            _ => direction
        };
    }

    // Get all valid direction aliases for display
    public static string GetValidDirections()
    {
        return "north (n), south (s), east (e), west (w), up (u), down (d), " +
               "northeast (ne), northwest (nw), southeast (se), southwest (sw)";
    }
}
