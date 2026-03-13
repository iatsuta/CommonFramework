using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static IServiceProxyFactory GetServiceProxyFactory(this IServiceProvider serviceProvider) =>
        serviceProvider.GetRequiredService<IServiceProxyFactory>();
}