public enum CommandType
{
    Unknown, Help, Go, Quit, Look, Health, Take, Place, Drop, Backpack, Use, Craft, Attack, Heal, Damage, Die, Cast, Spells
}

public static class CommandTypeExtensions
{
    private static readonly Dictionary<string, CommandType> commandMap =
        Enum.GetValues<CommandType>()
            .Where(t => t != CommandType.Unknown)
            .ToDictionary(t => t.ToString().ToLower(), t => t);

    private static readonly Dictionary<CommandType, string> descriptions = new()
    {
        [CommandType.Help] = "Display help information",
        [CommandType.Go] = "Move to another room (requires direction)",
        [CommandType.Quit] = "Exit the game",
        [CommandType.Look] = "Look around the current room",
        [CommandType.Health] = "Check your current health",
        [CommandType.Take] = "Take an item from the room",
        [CommandType.Place] = "Place an item in the room",
        [CommandType.Drop] = "Drop an item from your backpack",
        [CommandType.Backpack] = "Check your backpack",
        [CommandType.Use] = "Use an item",
        [CommandType.Craft] = "Craft an item",
        [CommandType.Attack] = "Attack an enemy",
        [CommandType.Heal] = "Heal yourself",
        [CommandType.Damage] = "Take damage (debug)",
        [CommandType.Die] = "Kill yourself (debug)",
        [CommandType.Cast] = "Cast a spell",
        [CommandType.Spells] = "View known spells"
    };

    public static CommandType FromString(string command) =>
        string.IsNullOrEmpty(command) ? CommandType.Unknown
        : commandMap.TryGetValue(command.ToLower(), out var type) ? type
        : CommandType.Unknown;

    public static string ToCommandString(this CommandType type) =>
        type == CommandType.Unknown ? "unknown" : type.ToString().ToLower();

    public static string GetDescription(this CommandType type) =>
        descriptions.TryGetValue(type, out var desc) ? desc : "Unknown command";

    public static bool RequiresParameter(this CommandType type) => type switch
    {
        CommandType.Go or CommandType.Take or CommandType.Place or CommandType.Drop or
        CommandType.Use or CommandType.Craft or CommandType.Heal or
        CommandType.Damage or CommandType.Cast => true,
        _ => false
    };

    public static IEnumerable<CommandType> GetValidCommands() =>
        Enum.GetValues<CommandType>().Where(t => t != CommandType.Unknown);
}
