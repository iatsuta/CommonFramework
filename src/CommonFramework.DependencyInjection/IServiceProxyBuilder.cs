namespace CommonFramework.DependencyInjection;

public interface IServiceProxyBuilder
{
    IServiceProxyBuilder AddRedirect(Type from, Type to);
}