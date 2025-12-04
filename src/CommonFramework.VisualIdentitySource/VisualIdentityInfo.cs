using System.Linq.Expressions;

namespace CommonFramework.VisualIdentitySource;

public record VisualIdentityInfo<TDomainObject>(PropertyAccessors<TDomainObject, string> Name) : VisualIdentityInfo
{
	public VisualIdentityInfo(Expression<Func<TDomainObject, string>> namePath) :
		this(namePath.ToPropertyAccessors())
	{
	}

	public override Type DomainObjectType { get; } = typeof(TDomainObject);
}

public abstract record VisualIdentityInfo
{
	public abstract Type DomainObjectType { get; }
}