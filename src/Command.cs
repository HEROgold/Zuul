public readonly record struct Command(CommandType Type, IParameter Parameter = null)
{
    public bool IsUnknown() => Type == CommandType.Unknown;
    public bool HasParameter() => Parameter != null;
    public bool IsMissingRequiredParameter() => Type.RequiresParameter() && !HasParameter();
}
