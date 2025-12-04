namespace CommonFramework.VisualIdentitySource;

public record VisualIdentityPropertySourceSettings(IReadOnlyList<string> PropertyNameList)
{
	public static VisualIdentityPropertySourceSettings Default { get; } = new(["Login", "Name", "Code"]);
}