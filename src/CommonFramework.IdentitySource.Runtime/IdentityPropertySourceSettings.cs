namespace CommonFramework.IdentitySource;

public record IdentityPropertySourceSettings(string PropertyName)
{
	public static IdentityPropertySourceSettings Default { get; } = new("Id");
}