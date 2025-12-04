using System.Reflection;

namespace CommonFramework.VisualIdentitySource;

public interface IVisualIdentityPropertyExtractor
{
	PropertyInfo? TryExtract(Type domainType);
}