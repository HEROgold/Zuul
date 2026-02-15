class DirectionMenuSystem(IEnumerable<Direction> directions) : BaseMenuSystem<Direction?>(directions.Cast<Direction?>())
{
    protected override string GetTitle() => "Select a Direction";

    protected override string FormatOption(Direction? dir) => dir?.ToDirectionString() ?? "None";

    protected override Direction? GetCancelValue() => null;
}
