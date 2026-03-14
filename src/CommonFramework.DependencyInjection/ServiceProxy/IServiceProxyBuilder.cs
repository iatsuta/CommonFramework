namespace CommonFramework.DependencyInjection.ServiceProxy;

public interface IServiceProxyBuilder
{
    IServiceProxyBuilder SetRedirect(Type sourceType, Type targetType, bool replace) => this.SetRedirect(sourceType, _ => targetType, replace);

    IServiceProxyBuilder SetRedirect(Type sourceType, Func<IServiceProvider, Type> targetTypeSelector, bool replace);

    IServiceProxyBuilder BindRedirect(Type sourceType, Type binderType, bool replace);
}