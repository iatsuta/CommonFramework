namespace CommonFramework.GenericRepository;

public interface IQueryableSource
{
	IQueryable<TDomainObject> GetQueryable<TDomainObject>()
		where TDomainObject : class;
}