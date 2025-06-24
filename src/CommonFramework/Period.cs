namespace CommonFramework;

public record struct Period(DateTime StartDate, DateTime? EndDate)
{
    public static readonly Period Eternity = new(DateTime.MinValue, null);
}