using System.Linq.Expressions;

namespace CommonFramework.IdentitySource.DependencyInjection;

public interface IIdentitySourceBuilder
{
	IIdentitySourceBuilder SetSettings(IdentityPropertySourceSettings settings);

	IIdentitySourceBuilder SetId<TDomainObject, TIdent>(Expression<Func<TDomainObject, TIdent>> idPath)
		where TIdent : notnull;
}