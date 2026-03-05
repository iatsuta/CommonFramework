using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace CommonFramework.IdentitySource;

public class IdentityInfoSource(IIdentityPropertyExtractor propertyExtractor, IEnumerable<IdentityInfo> customInfoList) : IIdentityInfoSource
{
    private readonly ConcurrentDictionary<Type, IdentityInfo> identityInfoCache = [];

    public IdentityInfo GetIdentityInfo(Type domainObjectType)
    {
        return this.identityInfoCache.GetOrAdd(domainObjectType, _ =>
        {
            var customInfo = customInfoList.SingleOrDefault(identityInfo => identityInfo.DomainObjectType == domainObjectType);

            if (customInfo != null)
            {
                return customInfo;
            }
            else
            {
                var idProperty = propertyExtractor.Extract(domainObjectType);

                var idPath = idProperty.ToGetLambdaExpression();

                return new Func<Expression<Func<object, object>>, IdentityInfo<object, object>>(CreateIdentityInfo)
                    .CreateGenericMethod(domainObjectType, idProperty.PropertyType)
                    .Invoke<IdentityInfo>(null, idPath);
            }

        });
    }

    private static IdentityInfo<TDomainObject, TIdent> CreateIdentityInfo<TDomainObject, TIdent>(Expression<Func<TDomainObject, TIdent>> idPath)
        where TIdent : notnull
    {
        return new IdentityInfo<TDomainObject, TIdent>(idPath);
    }
}