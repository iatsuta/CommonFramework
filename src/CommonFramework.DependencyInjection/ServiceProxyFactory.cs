using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection;

public class ServiceProxyFactory(IServiceProvider serviceProvider) : IServiceProxyFactory
{
    public virtual TService Create<TService>(Type requiredService, object[] args)
    {
        return (TService)ActivatorUtilities.CreateInstance(serviceProvider, requiredService, args);
    }
}