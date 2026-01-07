public readonly struct Command
{
    public CommandType Type { get; init; }
    public string Parameter { get; init; }

    public Command(CommandType type, string parameter = null) => (Type, Parameter) = (type, parameter);

    public bool IsUnknown() => Type == CommandType.Unknown;
    public bool HasParameter() => !string.IsNullOrEmpty(Parameter);
    public bool IsMissingRequiredParameter() => Type.RequiresParameter() && !HasParameter();
}
