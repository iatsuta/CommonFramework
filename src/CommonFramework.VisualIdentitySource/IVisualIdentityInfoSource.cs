namespace CommonFramework.VisualIdentitySource;

public interface IVisualIdentityInfoSource
{
	VisualIdentityInfo<TDomainObject> GetVisualIdentityInfo<TDomainObject>();

	VisualIdentityInfo<TDomainObject>? TryGetVisualIdentityInfo<TDomainObject>();
}