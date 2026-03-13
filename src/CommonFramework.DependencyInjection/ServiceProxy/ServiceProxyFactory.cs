using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection.ServiceProxy;

public class ServiceProxyFactory(IServiceProvider serviceProvider, IServiceProxyTypeRedirector redirector) : IServiceProxyFactory
{
    public virtual TService Create<TService>(Type instanceServiceType, object[] args)
    {
        var realInstanceServiceType = redirector.TryRedirect(instanceServiceType) ?? instanceServiceType;

        return (TService)ActivatorUtilities.CreateInstance(serviceProvider, realInstanceServiceType, args);
    }
}