using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.IdentitySource.DependencyInjection;

public class IdentitySourceBuilder : IIdentitySourceBuilder, IServiceInitializer
{
    private IdentityPropertySourceSettings? customSettings;

    private readonly List<Action<IServiceCollection>> actions = [];

    public IIdentitySourceBuilder SetSettings(IdentityPropertySourceSettings settings)
    {
        this.customSettings = settings;

        return this;
    }

    public IIdentitySourceBuilder SetId<TDomainObject, TIdent>(Expression<Func<TDomainObject, TIdent>> idPath)
        where TIdent : notnull
    {
        var identityInfo = new IdentityInfo<TDomainObject, TIdent>(idPath);

        this.actions.Add(sc => sc.AddSingleton(identityInfo).AddSingleton<IdentityInfo<TDomainObject>>(identityInfo).AddSingleton<IdentityInfo>(identityInfo));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        if (services.AlreadyInitialized<IIdentityInfoSource>())
        {
            if (this.customSettings != null)
            {
                services.ReplaceSingleton(this.customSettings);
            }
        }
        else
        {
            services.AddSingleton(typeof(IIdentityInfo<>), typeof(IdentityInfoProxy<>));
            services.AddSingleton(typeof(IIdentityInfo<,>), typeof(IdentityInfoProxy<,>));

            services.AddSingleton<IIdentityInfoSource, IdentityInfoSource>();
            services.AddSingleton<IIdentityPropertyExtractor, IdentityPropertyExtractor>();

            services.AddSingleton(this.customSettings ?? IdentityPropertySourceSettings.Default);
        }

        foreach (var action in this.actions)
        {
            action(services);
        }
    }
}