using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommonFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection TryAddServiceProxyFactory()
        {
            services.TryAddTransient<IServiceProxyFactory, ServiceProxyFactory>();

            return services;
        }


        public IServiceCollection AddScopedFrom<TService, TServiceImplementation>()
		    where TServiceImplementation : class, TService
		    where TService : class
	    {
		    return services.AddScopedFrom<TService, TServiceImplementation>(v => v);
	    }

	    public IServiceCollection AddScopedFrom<TService, TSource>(Func<TSource, TService> selector)
		    where TService : class
		    where TSource : notnull
	    {
		    return services.AddScoped(sp => selector(sp.GetRequiredService<TSource>()));
        }

        public IServiceCollection AddScopedFrom<TService>(Func<IServiceProxyFactory, TService> selector)
            where TService : class
        {
            return services.AddScopedFrom<TService, IServiceProxyFactory>(selector);
        }

        public IServiceCollection AddScopedFrom<TService>(Func<IServiceProvider, TService> selector)
            where TService : class
        {
            return services.AddScopedFrom<TService, IServiceProvider>(selector);
        }




        public IServiceCollection AddSingletonFrom<TService, TServiceImplementation>()
		    where TServiceImplementation : class, TService
		    where TService : class
	    {
		    return services.AddSingletonFrom<TService, TServiceImplementation>(v => v);
	    }

	    public IServiceCollection AddSingletonFrom<TService, TSource>(Func<TSource, TService> selector)
		    where TService : class
		    where TSource : notnull
	    {
		    return services.AddSingleton(sp => selector(sp.GetRequiredService<TSource>()));
        }

        public IServiceCollection AddSingletonFrom<TService>(Func<IServiceProxyFactory, TService> selector)
            where TService : class
        {
            return services.AddSingletonFrom<TService, IServiceProxyFactory>(selector);
        }

        public IServiceCollection AddSingletonFrom<TService>(Func<IServiceProvider, TService> selector)
            where TService : class
        {
            return services.AddSingletonFrom<TService, IServiceProvider>(selector);
        }



        public IServiceCollection ReplaceScoped<TService, TServiceImplementation>()
		    where TServiceImplementation : class, TService
		    where TService : class
	    {
		    return services.Replace(ServiceDescriptor.Scoped<TService, TServiceImplementation>());
	    }



        public IServiceCollection ReplaceScopedFrom<TService, TSource>(Func<TSource, TService> selector)
            where TService : class
            where TSource : notnull
        {
            return services.ReplaceScopedFrom(sp => selector(sp.GetRequiredService<TSource>()));
        }

        public IServiceCollection ReplaceScopedFrom<TService, TServiceImplementation>()
		    where TServiceImplementation : class, TService
		    where TService : class
	    {
		    return services.Replace(ServiceDescriptor.Scoped<TService>(sp => sp.GetRequiredService<TServiceImplementation>()));
        }

        public IServiceCollection ReplaceScopedFrom<TService>(Func<IServiceProxyFactory, TService> selector)
            where TService : class
        {
            return services.ReplaceScopedFrom<TService, IServiceProxyFactory>(selector);
        }

        public IServiceCollection ReplaceScopedFrom<TService>(Func<IServiceProvider, TService> selector)
            where TService : class
        {
            return services.ReplaceScopedFrom<TService, IServiceProvider>(selector);
        }



        public IServiceCollection ReplaceSingleton<TService>(TService instance)
		    where TService : class
	    {
		    return services.Replace(ServiceDescriptor.Singleton(instance));
	    }

	    public IServiceCollection ReplaceSingleton<TService, TServiceImplementation>()
		    where TServiceImplementation : class, TService
		    where TService : class
	    {
		    return services.Replace(ServiceDescriptor.Singleton<TService, TServiceImplementation>());
	    }




	    public IServiceCollection ReplaceSingletonFrom<TService, TServiceImplementation>()
		    where TServiceImplementation : class, TService
		    where TService : class
	    {
		    return services.Replace(ServiceDescriptor.Singleton<TService>(sp => sp.GetRequiredService<TServiceImplementation>()));
	    }

        public IServiceCollection ReplaceSingletonFrom<TService, TSource>(Func<TSource, TService> selector)
            where TService : class
            where TSource : notnull
        {
            return services.ReplaceSingletonFrom(sp => selector(sp.GetRequiredService<TSource>()));
        }

        public IServiceCollection ReplaceSingletonFrom<TService>(Func<IServiceProxyFactory, TService> selector)
            where TService : class
        {
            return services.ReplaceSingletonFrom<TService, IServiceProxyFactory>(selector);
        }

        public IServiceCollection ReplaceSingletonFrom<TService>(Func<IServiceProvider, TService> selector)
            where TService : class
        {
            return services.ReplaceSingletonFrom<TService, IServiceProvider>(selector);
        }
    }
}