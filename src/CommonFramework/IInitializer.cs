namespace CommonFramework;

public interface IInitializer
{
    Task Initialize(CancellationToken cancellationToken = default);
}