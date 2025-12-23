using System.Linq.Expressions;

using CommonFramework.DictionaryCache;

namespace CommonFramework.VisualIdentitySource;

public class VisualIdentityInfoSource(IVisualIdentityPropertyExtractor propertyExtractor, IEnumerable<VisualIdentityInfo> customInfoList)
	: IVisualIdentityInfoSource
{
	private readonly IDictionaryCache<Type, VisualIdentityInfo?> cache = new DictionaryCache<Type, VisualIdentityInfo?>(domainType =>
	{
		var customInfo = customInfoList.SingleOrDefault(identityInfo => identityInfo.DomainObjectType == domainType);

		if (customInfo != null)
		{
			return customInfo;
		}
		else
		{
			var nameProperty = propertyExtractor.TryExtract(domainType);

			if (nameProperty == null)
			{
				return null;
			}
			else
			{
				var idPath = nameProperty.ToGetLambdaExpression();

				return new Func<Expression<Func<object, string>>, VisualIdentityInfo<object>>(CreateVisualIdentityInfo)
					.CreateGenericMethod(domainType)
					.Invoke<VisualIdentityInfo>(null, idPath);
			}
		}

	}).WithLock();
    public VisualIdentityInfo<TDomainObject>? TryGetVisualIdentityInfo<TDomainObject>()
    {
        return (VisualIdentityInfo<TDomainObject>?)this.TryGetVisualIdentityInfo(typeof(TDomainObject));
    }

    public VisualIdentityInfo GetVisualIdentityInfo(Type domainObjectType)
    {
        return this.TryGetVisualIdentityInfo(domainObjectType) ?? throw this.GetMissedError(domainObjectType);
    }

    public VisualIdentityInfo? TryGetVisualIdentityInfo(Type domainObjectType)
    {
        return this.cache[domainObjectType];
    }

    public VisualIdentityInfo<TDomainObject> GetVisualIdentityInfo<TDomainObject>()
	{
		return this.TryGetVisualIdentityInfo<TDomainObject>() ?? throw this.GetMissedError(typeof(TDomainObject));
	}

    private Exception GetMissedError(Type domainObjectType)
    {
        return new Exception($"{nameof(VisualIdentityInfo)} for {domainObjectType.Name} not found");
    }

    private static VisualIdentityInfo<TDomainObject> CreateVisualIdentityInfo<TDomainObject>(Expression<Func<TDomainObject, string>> namePath)
	{
		return new VisualIdentityInfo<TDomainObject>(namePath);
	}
}