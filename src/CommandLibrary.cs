class CommandLibrary
{
    private readonly List<CommandType> validCommands = [.. CommandTypeExtensions.GetValidCommands()];

    public bool IsValidCommand(CommandType type) => type != CommandType.Unknown && validCommands.Contains(type);
    public List<CommandType> GetCommandTypes() => [.. validCommands];
    public List<string> GetCommandsList() => [.. validCommands.Select(cmd => cmd.ToCommandString())];
    public string GetCommandsString() => string.Join(", ", validCommands.Select(cmd => cmd.ToCommandString()));
}
