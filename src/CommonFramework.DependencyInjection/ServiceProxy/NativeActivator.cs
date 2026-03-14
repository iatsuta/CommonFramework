using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection.ServiceProxy;

public class NativeActivator(IServiceProvider serviceProvider) : INativeActivator
{
    public virtual TService Create<TService>(Type instanceServiceType, object[] args)
    {
        return (TService)ActivatorUtilities.CreateInstance(serviceProvider, instanceServiceType, args);
    }
}