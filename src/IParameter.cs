public interface IParameter { }

public readonly record struct ItemParameter(Item Item) : IParameter;

public readonly record struct DirectionParameter(Direction Direction) : IParameter;

public readonly record struct NumberParameter(int Value) : IParameter;
