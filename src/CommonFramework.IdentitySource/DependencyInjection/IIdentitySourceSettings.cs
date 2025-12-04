using System.Linq.Expressions;

namespace CommonFramework.IdentitySource.DependencyInjection;

public interface IIdentitySourceSettings
{
	IIdentitySourceSettings SetSettings(IdentityPropertySourceSettings settings);

	IIdentitySourceSettings SetId<TDomainObject, TIdent>(Expression<Func<TDomainObject, TIdent>> idPath)
		where TIdent : notnull;
}