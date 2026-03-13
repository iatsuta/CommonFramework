using System.Linq.Expressions;

namespace CommonFramework.IdentitySource;

public interface IIdentityInfo<TDomainObject, TIdent> : IIdentityInfo<TDomainObject>
{
    PropertyAccessors<TDomainObject, TIdent> Id { get; }

    Expression<Func<TDomainObject, bool>> CreateContainsFilter(IEnumerable<TIdent> idents);
}

public interface IIdentityInfo<in TDomainObject>
{
    Type IdentityType { get; }

    object GetId(TDomainObject domainObject);
}
