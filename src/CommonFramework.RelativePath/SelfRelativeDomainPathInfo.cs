namespace CommonFramework.RelativePath;

public record SelfRelativeDomainPathInfo<T>() : SingleRelativeDomainPathInfo<T, T>(v => v);
