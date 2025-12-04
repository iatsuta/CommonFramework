using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.IdentitySource.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentitySource(this IServiceCollection services, Action<IIdentitySourceSettings>? setup = null)
    {
	    var settings = new IdentitySourceSettings();

        setup?.Invoke(settings);

		settings.Initialize(services);

		return services;
    }
}