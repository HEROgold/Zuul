using System.Collections.Generic;
using System.Linq;

class CommandLibrary
{
    private readonly List<CommandType> validCommands = new(CommandTypeExtensions.GetValidCommands());

    public bool IsValidCommand(CommandType type) => type != CommandType.Unknown && validCommands.Contains(type);
    public List<CommandType> GetCommandTypes() => new(validCommands);
    public List<string> GetCommandsList() => validCommands.Select(cmd => cmd.ToCommandString()).ToList();
    public string GetCommandsString() => string.Join(", ", validCommands.Select(cmd => cmd.ToCommandString()));
}
