using System.Reflection;

namespace CommonFramework.IdentitySource;

public class IdentityPropertyExtractor(IdentityPropertySourceSettings settings) : IIdentityPropertyExtractor
{
	public PropertyInfo Extract(Type domainType)
	{
		return domainType.GetRequiredProperty(settings.PropertyName, BindingFlags.Public | BindingFlags.Instance);
	}
}