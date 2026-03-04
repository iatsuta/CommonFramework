using System.Linq.Expressions;

namespace CommonFramework.VisualIdentitySource.DependencyInjection;

public interface IVisualIdentitySourceBuilder
{
	IVisualIdentitySourceBuilder SetSettings(VisualIdentityPropertySourceSettings settings);

	IVisualIdentitySourceBuilder SetName<TDomainObject>(Expression<Func<TDomainObject, string>> namePath);

	IVisualIdentitySourceBuilder SetDisplay<TDomainObject>(Func<TDomainObject, string> displayFunc)
		where TDomainObject : class;
}