using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection;

public class ServiceProxyBuilder : IServiceProxyBuilder
{
    private readonly List<ServiceProxyTypeRedirectInfo> redirects = new();

    public IServiceProxyBuilder AddRedirect(Type from, Type to)
    {
        this.redirects.Add(new ServiceProxyTypeRedirectInfo(from, to));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        foreach (var redirectInfo in this.redirects)
        {
            services.AddSingleton(redirectInfo);
        }
    }
}