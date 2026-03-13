namespace CommonFramework.VisualIdentitySource;

public class DomainObjectDisplayService(IDisplayObjectInfoSource displayObjectInfoSource) : IDomainObjectDisplayService
{
    public string ToString<TDomainObject>(TDomainObject domainObject)
        where TDomainObject : class =>
        displayObjectInfoSource.GetDisplayObjectInfo<TDomainObject>().DisplayFunc(domainObject);
}