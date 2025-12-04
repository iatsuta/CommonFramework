using System.Reflection;

namespace CommonFramework.IdentitySource;

public interface IIdentityPropertyExtractor
{
    PropertyInfo Extract(Type domainType);
}