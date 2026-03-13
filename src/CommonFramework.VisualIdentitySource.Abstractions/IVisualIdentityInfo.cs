namespace CommonFramework.VisualIdentitySource;

public interface IVisualIdentityInfo<TDomainObject>
{
    PropertyAccessors<TDomainObject, string> Name { get; }
}