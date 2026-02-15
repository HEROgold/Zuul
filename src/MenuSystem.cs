class MenuSystem(List<CommandType> menuOptions) : BaseMenuSystem<CommandType>(menuOptions)
{
    protected override string GetTitle() => "Select a Command";

    protected override string FormatOption(CommandType cmd) => 
        $"{cmd.ToCommandString()} - {cmd.GetDescription()}";

    protected override CommandType GetCancelValue() => CommandType.Unknown;
}
