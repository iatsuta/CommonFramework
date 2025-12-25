namespace CommonFramework.DependencyInjection;

public interface IServiceProxyTypeRedirector
{
    Type? TryRedirect(Type type);
}