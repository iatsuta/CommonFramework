namespace CommonFramework.Maybe;

public record Nothing<T> : Maybe<T>
{
    public override bool HasValue { get; } = false;

    public override string ToString()
    {
        return "";
    }
}