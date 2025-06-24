namespace CommonFramework.Maybe;

public record Just<T>(T Value) : Maybe<T>
{
    public override bool HasValue { get; } = true;

    public override string ToString()
    {
        return $"{this.Value}";
    }
}