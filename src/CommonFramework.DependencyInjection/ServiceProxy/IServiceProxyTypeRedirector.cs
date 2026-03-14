namespace CommonFramework.DependencyInjection.ServiceProxy;

public interface IServiceProxyTypeRedirector
{
    Type? TryRedirect(Type sourceType);
}