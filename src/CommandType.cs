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
    Health
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
        { "health", CommandType.Health }
    };

    private static readonly Dictionary<CommandType, string> commandToStringMap = new()
    {
        { CommandType.Help, "help" },
        { CommandType.Go, "go" },
        { CommandType.Quit, "quit" },
        { CommandType.Look, "look" },
        { CommandType.Health, "health" }
    };

    private static readonly Dictionary<CommandType, string> commandDescriptions = new()
    {
        { CommandType.Help, "Display help information" },
        { CommandType.Go, "Move to another room (requires direction)" },
        { CommandType.Quit, "Exit the game" },
        { CommandType.Look, "Look around the current room" },
        { CommandType.Health, "Check your current health" }
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
        return type == CommandType.Go;
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
            CommandType.Health
        };
    }
}
