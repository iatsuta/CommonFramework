namespace CommonFramework.DependencyInjection.ServiceProxy;

public interface IServiceProxyBuilder
{
    IServiceProxyBuilder SetRedirect(Type from, Type to) => this.SetRedirect(from, _ => to);

    IServiceProxyBuilder SetRedirect(Type from, Func<IServiceProvider, Type> toSelector);

    IServiceProxyBuilder BindRedirect<TServiceProxyBinder>(Type from)
        where TServiceProxyBinder : class, IServiceProxyBinder;
}