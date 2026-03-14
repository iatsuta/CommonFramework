namespace CommonFramework.VisualIdentitySource;

public class VisualIdentityInfoProxy<TDomainObject>(IVisualIdentityInfoSource visualIdentityInfoSource) : IVisualIdentityInfo<TDomainObject>
{
    private readonly VisualIdentityInfo<TDomainObject> innerInfo = visualIdentityInfoSource.GetVisualIdentityInfo<TDomainObject>();

    public Type DomainObjectType => innerInfo.DomainObjectType;

    public PropertyAccessors<TDomainObject, string> Name => innerInfo.Name;
}