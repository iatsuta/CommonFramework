namespace CommonFramework.VisualIdentitySource;

public interface IDomainObjectDisplayService
{
	string ToString<TDomainObject>(TDomainObject domainObject)
		where TDomainObject : class;
}