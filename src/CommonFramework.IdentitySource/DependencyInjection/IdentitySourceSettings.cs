using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.IdentitySource.DependencyInjection;

public class IdentitySourceSettings : IIdentitySourceSettings
{
	private IdentityPropertySourceSettings? customSettings;

	private readonly List<Action<IServiceCollection>> actions = new();

	public IIdentitySourceSettings SetSettings(IdentityPropertySourceSettings settings)
	{
		this.customSettings = settings;

		return this;
	}

	public IIdentitySourceSettings SetId<TDomainObject, TIdent>(Expression<Func<TDomainObject, TIdent>> idPath)
		where TIdent : notnull
	{
		this.actions.Add(sc => sc.AddSingleton(new IdentityInfo<TDomainObject, TIdent>(idPath)));

		return this;
	}

	public void Initialize(IServiceCollection services)
	{
		if (this.AlreadyInitialized(services))
		{
			if (this.customSettings != null)
			{
				services.ReplaceSingleton(this.customSettings);
			}
		}
		else
		{
			services.AddSingleton<IIdentityInfoSource, IdentityInfoSource>();
			services.AddSingleton<IIdentityPropertyExtractor, IdentityPropertyExtractor>();

			services.AddSingleton(this.customSettings ?? IdentityPropertySourceSettings.Default);
		}

		foreach (var action in this.actions)
		{
			action(services);
		}
	}

	private bool AlreadyInitialized(IServiceCollection services)
	{
		return services.Any(sd => !sd.IsKeyedService && sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(IIdentityInfoSource));
	}
}