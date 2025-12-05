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
		var visualIdentityInfo = new VisualIdentityInfo<TDomainObject>(namePath);

		this.actions.Add(sc => sc.AddSingleton(visualIdentityInfo).AddSingleton<VisualIdentityInfo>(visualIdentityInfo));

		return this;
	}

	public IVisualIdentitySourceSettings SetDisplay<TDomainObject>(Func<TDomainObject, string> displayFunc)
		where TDomainObject : class
	{
		var displayObjectInfo = new DisplayObjectInfo<TDomainObject>(displayFunc);

		this.actions.Add(sc => sc.AddSingleton(displayObjectInfo).AddSingleton<DisplayObjectInfo>(displayObjectInfo));

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
			services.AddSingleton<IVisualIdentityInfoSource, VisualIdentityInfoSource>();
			services.AddSingleton<IVisualIdentityPropertyExtractor, VisualIdentityPropertyExtractor>();

			services.AddSingleton<IDomainObjectDisplayService, DomainObjectDisplayService>();

			services.AddSingleton(this.customSettings ?? VisualIdentityPropertySourceSettings.Default);
		}

		foreach (var action in this.actions)
		{
			action(services);
		}
	}

	private bool AlreadyInitialized(IServiceCollection services)
	{
		return services.Any(sd => !sd.IsKeyedService && sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(IVisualIdentityInfoSource));
	}
}