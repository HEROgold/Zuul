public enum Direction
{
    North, South, East, West, Up, Down, Northeast, Northwest, Southeast, Southwest
}

public static class DirectionExtensions
{
    private static readonly Dictionary<string, Direction> directionMap =
        Enum.GetValues<Direction>()
            .SelectMany(d => GetAliases(d).Select(alias => (alias, d)))
            .ToDictionary(t => t.alias, t => t.d);

    private static string[] GetAliases(Direction direction) => direction switch
    {
        Direction.North => ["north", "n"],
        Direction.South => ["south", "s"],
        Direction.East => ["east", "e"],
        Direction.West => ["west", "w"],
        Direction.Up => ["up", "u"],
        Direction.Down => ["down", "d"],
        Direction.Northeast => ["northeast", "ne"],
        Direction.Northwest => ["northwest", "nw"],
        Direction.Southeast => ["southeast", "se"],
        Direction.Southwest => ["southwest", "sw"],
        _ => []
    };

    public static Direction? FromString(string directionString) =>
        string.IsNullOrWhiteSpace(directionString) ? null
        : directionMap.TryGetValue(directionString.ToLower().Trim(), out var direction) ? direction
        : null;

    public static string ToDirectionString(this Direction direction) =>
        direction.ToString().ToLower();

    public static Direction GetOpposite(this Direction direction) => direction switch
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
