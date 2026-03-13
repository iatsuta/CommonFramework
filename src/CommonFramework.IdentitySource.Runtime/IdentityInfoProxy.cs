using System.Linq.Expressions;

namespace CommonFramework.IdentitySource;

public class IdentityInfoProxy<TDomainObject, TIdent>(IIdentityInfoSource identityInfoSource) : IIdentityInfo<TDomainObject, TIdent>
    where TIdent : notnull
{
    private readonly IIdentityInfo<TDomainObject, TIdent> innerInfo = identityInfoSource.GetIdentityInfo<TDomainObject, TIdent>();

    public object GetId(TDomainObject domainObject) => innerInfo.GetId(domainObject);

    public PropertyAccessors<TDomainObject, TIdent> Id => innerInfo.Id;

    public Expression<Func<TDomainObject, bool>> CreateContainsFilter(IEnumerable<TIdent> idents) => innerInfo.CreateContainsFilter(idents);
}