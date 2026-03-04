using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.VisualIdentitySource.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVisualIdentitySource(this IServiceCollection services, Action<IVisualIdentitySourceBuilder>? setup = null) =>
        services.Initialize<VisualIdentitySourceBuilder>(setup);
}