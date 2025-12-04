using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.VisualIdentitySource.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVisualIdentitySource(this IServiceCollection services, Action<IVisualIdentitySourceSettings>? setup = null)
    {
	    var settings = new VisualIdentitySourceSettings();

        setup?.Invoke(settings);

		settings.Initialize(services);

		return services;
    }
}