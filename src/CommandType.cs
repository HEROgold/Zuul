using System;
using System.Collections.Generic;

// Enum for all valid command types
public enum CommandType
{
    Unknown,
    Help,
    Go,
    Quit,
    Look,
    Health,
    Take,
    Place,
    Backpack,
    Use,
    Craft,
    Attack,
    Heal,
    Damage,
    Die
}

// Extension methods for CommandType
public static class CommandTypeExtensions
{
    private static readonly Dictionary<string, CommandType> stringToCommandMap = new()
    {
        { "help", CommandType.Help },
        { "go", CommandType.Go },
        { "quit", CommandType.Quit },
        { "look", CommandType.Look },
        { "health", CommandType.Health },
        { "take", CommandType.Take },
        { "place", CommandType.Place },
        { "backpack", CommandType.Backpack },
        { "use", CommandType.Use },
        { "craft", CommandType.Craft },
        { "attack", CommandType.Attack },
        { "heal", CommandType.Heal },
        { "damage", CommandType.Damage },
        { "die", CommandType.Die }
    };

    private static readonly Dictionary<CommandType, string> commandToStringMap = new()
    {
        { CommandType.Help, "help" },
        { CommandType.Go, "go" },
        { CommandType.Quit, "quit" },
        { CommandType.Look, "look" },
        { CommandType.Health, "health" },
        { CommandType.Take, "take" },
        { CommandType.Place, "place" },
        { CommandType.Backpack, "backpack" },
        { CommandType.Use, "use" },
        { CommandType.Craft, "craft" },
        { CommandType.Attack, "attack" },
        { CommandType.Heal, "heal" },
        { CommandType.Damage, "damage" },
        { CommandType.Die, "die" }
    };

    private static readonly Dictionary<CommandType, string> commandDescriptions = new()
    {
        { CommandType.Help, "Display help information" },
        { CommandType.Go, "Move to another room (requires direction)" },
        { CommandType.Quit, "Exit the game" },
        { CommandType.Look, "Look around the current room" },
        { CommandType.Health, "Check your current health" },
        { CommandType.Take, "Take an item from the room" },
        { CommandType.Place, "Place an item in the room" },
        { CommandType.Backpack, "Check your backpack" },
        { CommandType.Use, "Use an item" },
        { CommandType.Craft, "Craft an item" },
        { CommandType.Attack, "Attack an enemy" },
        { CommandType.Heal, "Heal yourself" },
        { CommandType.Damage, "Take damage (debug)" },
        { CommandType.Die, "Kill yourself (debug)" }
    };

    // Convert string to CommandType
    public static CommandType FromString(string command)
    {
        if (string.IsNullOrEmpty(command))
            return CommandType.Unknown;

        return stringToCommandMap.TryGetValue(command.ToLower(), out var type)
            ? type
            : CommandType.Unknown;
    }

    // Convert CommandType to string
    public static string ToCommandString(this CommandType type)
    {
        return commandToStringMap.TryGetValue(type, out var str) ? str : "unknown";
    }

    // Get description of command
    public static string GetDescription(this CommandType type)
    {
        return commandDescriptions.TryGetValue(type, out var desc) ? desc : "Unknown command";
    }

    // Check if command needs a second parameter
    public static bool RequiresParameter(this CommandType type)
    {
        return type switch
        {
            CommandType.Go => true,
            CommandType.Take => true,
            CommandType.Place => true,
            CommandType.Use => true,
            CommandType.Craft => true,
            CommandType.Heal => true,
            CommandType.Damage => true,
            _ => false
        };
    }

    // Get all valid command types (excluding Unknown)
    public static IEnumerable<CommandType> GetValidCommands()
    {
        return new[]
        {
            CommandType.Help,
            CommandType.Go,
            CommandType.Quit,
            CommandType.Look,
            CommandType.Health,
            CommandType.Take,
            CommandType.Place,
            CommandType.Backpack,
            CommandType.Use,
            CommandType.Craft,
            CommandType.Attack,
            CommandType.Heal,
            CommandType.Damage,
            CommandType.Die
        };
    }
}
