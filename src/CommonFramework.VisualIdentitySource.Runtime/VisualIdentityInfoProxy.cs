namespace CommonFramework.VisualIdentitySource;

public class VisualIdentityInfoProxy<TDomainObject>(IVisualIdentityInfoSource visualIdentityInfoSource) : IVisualIdentityInfo<TDomainObject>
{
    public PropertyAccessors<TDomainObject, string> Name { get; } = visualIdentityInfoSource.GetVisualIdentityInfo<TDomainObject>().Name;
}