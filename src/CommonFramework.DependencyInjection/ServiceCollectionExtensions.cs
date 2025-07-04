﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommonFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopedFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.AddScopedFrom<TService, TServiceImplementation>(v => v);
    }

    public static IServiceCollection AddScopedFrom<TService, TSource>(this IServiceCollection services, Func<TSource, TService> selector)
        where TService : class
        where TSource : notnull
    {
        return services.AddScoped(sp => selector(sp.GetRequiredService<TSource>()));
    }

    public static IServiceCollection AddSingletonFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.AddSingletonFrom<TService, TServiceImplementation>(v => v);
    }

    public static IServiceCollection AddSingletonFrom<TService, TSource>(this IServiceCollection services, Func<TSource, TService> selector)
        where TService : class
        where TSource : notnull
    {
        return services.AddSingleton(sp => selector(sp.GetRequiredService<TSource>()));
    }

    public static IServiceCollection ReplaceScoped<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Scoped<TService, TServiceImplementation>());
    }

    public static IServiceCollection ReplaceScopedFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Scoped<TService>(sp => sp.GetRequiredService<TServiceImplementation>()));
    }

    public static IServiceCollection ReplaceSingleton<TService>(this IServiceCollection services, TService instance)
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Singleton(instance));
    }

    public static IServiceCollection ReplaceSingleton<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Singleton<TService, TServiceImplementation>());
    }

    public static IServiceCollection ReplaceSingletonFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Singleton<TService>(sp => sp.GetRequiredService<TServiceImplementation>()));
    }
}
