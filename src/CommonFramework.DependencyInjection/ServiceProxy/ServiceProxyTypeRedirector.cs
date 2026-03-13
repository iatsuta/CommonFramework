using System.Collections.Concurrent;

namespace CommonFramework.DependencyInjection.ServiceProxy;

public class ServiceProxyTypeRedirector(IEnumerable<ServiceProxyTypeRedirectInfo> infoList) : IServiceProxyTypeRedirector
{
    private readonly Dictionary<Type, Type> baseCache = infoList.GroupBy(info => info.From, info => info.To).ToDictionary(g => g.Key, g => g.Last());

    private readonly ConcurrentDictionary<Type, Type?> cache = [];

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