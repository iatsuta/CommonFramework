using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.VisualIdentitySource.DependencyInjection;

public class VisualIdentitySourceSettings : IVisualIdentitySourceSettings
{
	private VisualIdentityPropertySourceSettings? customSettings;

	private readonly List<Action<IServiceCollection>> actions = new();

	public IVisualIdentitySourceSettings SetSettings(VisualIdentityPropertySourceSettings settings)
	{
		this.customSettings = settings;

		return this;
	}

	public IVisualIdentitySourceSettings SetName<TDomainObject>(Expression<Func<TDomainObject, string>> namePath)
	{
		this.actions.Add(sc => sc.AddSingleton(new VisualIdentityInfo<TDomainObject>(namePath)));

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
			services.AddSingleton(this.customSettings ?? VisualIdentityPropertySourceSettings.Default);
		}

		foreach (var action in this.actions)
		{
			action(services);
		}
	}

	private bool AlreadyInitialized(IServiceCollection services)
	{
		return services.Any(sd => !sd.IsKeyedService && sd.ServiceType == typeof(VisualIdentityPropertySourceSettings));
	}
}