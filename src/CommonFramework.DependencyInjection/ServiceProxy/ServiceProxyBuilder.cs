using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommonFramework.DependencyInjection.ServiceProxy;

public class ServiceProxyBuilder : IServiceProxyBuilder, IServiceInitializer
{
    private readonly List<Action<IServiceCollection>> registerRedirects = [];

    public IServiceProxyBuilder SetRedirect(Type from, Func<IServiceProvider, Type> toSelector)
    {
        this.registerRedirects.Add(sc => sc.AddSingleton(sp => new ServiceProxyTypeRedirectInfo(from, toSelector(sp))));

        return this;
    }

    public IServiceProxyBuilder BindRedirect<TServiceProxyBinder>(Type from)
        where TServiceProxyBinder : class, IServiceProxyBinder
    {
        this.registerRedirects.Add(sc => sc
            .AddSingleton<TServiceProxyBinder>()
            .AddSingletonFrom((TServiceProxyBinder binder) => new ServiceProxyTypeRedirectInfo(from, binder.GetTargetServiceType())));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.TryAddTransient<IServiceProxyFactory, ServiceProxyFactory>();
        services.TryAddSingleton<IServiceProxyTypeRedirector, ServiceProxyTypeRedirector>();

        foreach (var registerAction in this.registerRedirects)
        {
            registerAction(services);
        }
    }
}