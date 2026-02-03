using System.Reflection;

namespace CommonFramework.IdentitySource;

public class IdentityPropertyExtractor(IdentityPropertySourceSettings settings) : IIdentityPropertyExtractor
{
	public PropertyInfo Extract(Type domainType)
	{
		return domainType.GetProperty(settings.PropertyName) ?? throw new Exception($"{settings.PropertyName} property in {domainType.Name} not found");
	}
}