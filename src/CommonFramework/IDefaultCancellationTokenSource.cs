namespace CommonFramework;

public interface IDefaultCancellationTokenSource
{
    CancellationToken CancellationToken { get; }
}