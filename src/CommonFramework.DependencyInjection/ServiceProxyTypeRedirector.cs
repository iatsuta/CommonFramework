using System.Collections.Concurrent;

namespace CommonFramework.DependencyInjection;

public class ServiceProxyTypeRedirector(IEnumerable<ServiceProxyTypeRedirectInfo> infoList) : IServiceProxyTypeRedirector
{
    private readonly Dictionary<Type, Type> baseCache = infoList.ToDictionary(info => info.From, info => info.To);

    private readonly ConcurrentDictionary<Type, Type?> cache = new();

    public Type? TryRedirect(Type type)
    {
        return cache.GetOrAdd(type, _ =>
        {
            var directResult = this.baseCache.GetValueOrDefault(type) ;

            if (directResult == null && type.IsGenericType)
            {
                var genericResultType = baseCache.GetValueOrDefault(type.GetGenericTypeDefinition());

                if (genericResultType != null)
                {
                    return genericResultType.MakeGenericType(type.GetGenericArguments());
                }
            }

            return directResult;
        });
    }
}