using System.Linq.Expressions;

namespace CommonFramework.VisualIdentitySource.DependencyInjection;

public interface IVisualIdentitySourceSettings
{
	IVisualIdentitySourceSettings SetSettings(VisualIdentityPropertySourceSettings settings);

	IVisualIdentitySourceSettings SetName<TDomainObject>(Expression<Func<TDomainObject, string>> namePath);
}