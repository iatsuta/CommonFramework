using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.VisualIdentitySource.DependencyInjection;

public class VisualIdentitySourceBuilder : IVisualIdentitySourceBuilder, IServiceInitializer
{
    private VisualIdentityPropertySourceSettings? customSettings;

    private readonly List<Action<IServiceCollection>> actions = [];

    public IVisualIdentitySourceBuilder SetSettings(VisualIdentityPropertySourceSettings settings)
    {
        this.customSettings = settings;

        return this;
    }

    public IVisualIdentitySourceBuilder SetName<TDomainObject>(Expression<Func<TDomainObject, string>> namePath)
    {
        var visualIdentityInfo = new VisualIdentityInfo<TDomainObject>(namePath);

        this.actions.Add(sc => sc.AddSingleton(visualIdentityInfo).AddSingleton<VisualIdentityInfo>(visualIdentityInfo));

        return this;
    }

    public IVisualIdentitySourceBuilder SetDisplay<TDomainObject>(Func<TDomainObject, string> displayFunc)
        where TDomainObject : class
    {
        var displayObjectInfo = new DisplayObjectInfo<TDomainObject>(displayFunc);

        this.actions.Add(sc => sc.AddSingleton(displayObjectInfo).AddSingleton<DisplayObjectInfo>(displayObjectInfo));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        if (services.AlreadyInitialized<IVisualIdentityInfoSource>())
        {
            if (this.customSettings != null)
            {
                services.ReplaceSingleton(this.customSettings);
            }
        }
        else
        {
            services.AddSingleton(typeof(IVisualIdentityInfo<>), typeof(VisualIdentityInfoProxy<>));
            services.AddSingleton<IVisualIdentityInfoSource, VisualIdentityInfoSource>();
            services.AddSingleton<IVisualIdentityPropertyExtractor, VisualIdentityPropertyExtractor>();

            services.AddSingleton(typeof(IDisplayObjectInfo<>), typeof(DisplayObjectInfoProxy<>));
            services.AddSingleton<IDisplayObjectInfoSource, DisplayObjectInfoSource>();
            services.AddSingleton<IDomainObjectDisplayService, DomainObjectDisplayService>();

            services.AddSingleton(this.customSettings ?? VisualIdentityPropertySourceSettings.Default);
        }

        foreach (var action in this.actions)
        {
            action(services);
        }
    }
}