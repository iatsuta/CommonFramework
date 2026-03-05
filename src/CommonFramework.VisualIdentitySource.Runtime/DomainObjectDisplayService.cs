using System.Collections.Concurrent;

namespace CommonFramework.VisualIdentitySource;

public class DomainObjectDisplayService(IVisualIdentityInfoSource visualIdentityInfoSource, IEnumerable<DisplayObjectInfo> customDisplayObjectInfoList)
	: IDomainObjectDisplayService
{
	private readonly ConcurrentDictionary<Type, DisplayObjectInfo> cache = [];

	public string ToString<TDomainObject>(TDomainObject domainObject)
		where TDomainObject : class =>
        this.cache
            .GetOrAddAs(typeof(TDomainObject), _ => this.GetActualDisplayObjectInfo<TDomainObject>())
            .DisplayFunc(domainObject);

    private DisplayObjectInfo<TDomainObject> GetActualDisplayObjectInfo<TDomainObject>()
		where TDomainObject : class
	{
		if (customDisplayObjectInfoList.SingleOrDefault(info => info.DomainObjectType == typeof(TDomainObject)) is { } customInfo)
		{
			return (DisplayObjectInfo<TDomainObject>)customInfo;
		}
		else if (visualIdentityInfoSource.TryGetVisualIdentityInfo<TDomainObject>() is { } visualIdentityInfo)
		{
			return new DisplayObjectInfo<TDomainObject>(visualIdentityInfo.Name.Getter);
		}
		else
		{
			return DisplayObjectInfo<TDomainObject>.Default;
		}
	}
}