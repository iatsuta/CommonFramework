namespace CommonFramework;

public static class TypeExtensions
{
    public static bool IsGenericTypeImplementation(this Type type, Type genericTypeDefinition, Type[]? implementArguments = null)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition
                                  && (implementArguments == null || type.GetGenericArguments().SequenceEqual(implementArguments));
    }

    public static Type? GetInterfaceImplementationArgument(this Type type, Type interfaceType)
    {
        return type.GetInterfaceImplementationArguments(interfaceType, args => args.Single());
    }

    public static TResult? GetInterfaceImplementationArguments<TResult>(this Type type, Type interfaceType, Func<Type[], TResult> getResult)
        where TResult : class
    {
        return type.GetInterfaceImplementation(interfaceType).Maybe(i => getResult(i.GetGenericArguments()));
    }

    public static Type? GetInterfaceImplementation(this Type type, Type interfaceType)
    {
        return type.IsInterfaceImplementation(interfaceType)
            ? type.GetAllInterfaces().SingleOrDefault(i => IsGenericTypeImplementation(i, interfaceType))
            : null;
    }

    public static bool IsInterfaceImplementation(this Type type, Type interfaceType, Type[]? implementArguments = null)
    {
        if (!interfaceType.IsInterface) throw new ArgumentNullException($"Type \"{interfaceType.Name}\" is not interface");

        var interfaces = type.GetAllInterfaces().ToArray();

        return interfaceType.IsGenericTypeDefinition
            ? interfaces.Any(i => i.IsGenericTypeImplementation(interfaceType, implementArguments))
            : interfaces.Contains(interfaceType);
    }

    public static IEnumerable<Type> GetAllInterfaces(this Type type, bool unwrapSubInterfaceGenerics = true)
    {
        return type.IsInterface ? new[] { type }.Concat(type.GetInterfaces().Pipe(type.IsGenericTypeDefinition && unwrapSubInterfaceGenerics, res => res.Select(subType => subType.IsGenericType ? subType.GetGenericTypeDefinition() : subType).ToArray()))
            : type.GetInterfaces();
    }

    public static Type? GetGenericTypeImplementationArgument(this Type type, Type genericTypeDefinition)
    {
        return type.GetGenericTypeImplementationArguments(genericTypeDefinition, args => args.Single());
    }

    public static TResult? GetGenericTypeImplementationArguments<TResult>(this Type type, Type genericTypeDefinition, Func<Type[], TResult> getResult)
        where TResult : class
    {
        return type.IsGenericTypeImplementation(genericTypeDefinition)
            ? getResult(type.GetGenericArguments())
            : null;
    }
}