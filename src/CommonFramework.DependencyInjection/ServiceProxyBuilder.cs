using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommonFramework.DependencyInjection;

public class ServiceProxyBuilder : IServiceProxyBuilder, IServiceInitializer
{
    private readonly List<ServiceProxyTypeRedirectInfo> redirects = [];

    public IServiceProxyBuilder AddRedirect(Type from, Type to)
    {
        this.redirects.Add(new ServiceProxyTypeRedirectInfo(from, to));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.TryAddTransient<IServiceProxyFactory, ServiceProxyFactory>();
        services.TryAddSingleton<IServiceProxyTypeRedirector, ServiceProxyTypeRedirector>();

        foreach (var redirectInfo in this.redirects)
        {
            services.AddSingleton(redirectInfo);
        }
    }
}