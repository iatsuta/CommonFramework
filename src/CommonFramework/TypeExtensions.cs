using CommonFramework.Maybe;

using System.Collections.ObjectModel;
using System.Reflection;

namespace CommonFramework;

public static class TypeExtensions
{
    private static readonly HashSet<Type> CollectionTypes = new[]
    {
        typeof(IEnumerable<>),
        typeof(List<>),
        typeof(Collection<>),
        typeof(IList<>),
        typeof(ICollection<>),
        typeof(ObservableCollection<>),
        typeof(IReadOnlyList<>),
        typeof(IReadOnlyCollection<>)
    }.ToHashSet();

    public static Type? GetNullableElementType(this Type type)
    {
        return type.IsGenericTypeImplementation(typeof(Nullable<>)) ? type.GetGenericArguments().Single() : null;
    }

    public static Type GetCollectionElementTypeOrSelf(this Type type)
    {
        return type.GetCollectionElementType() ?? type;
    }

    public static Type? GetCollectionElementType(this Type type)
    {
        return type.GetCollectionType() != null ? type.GetGenericArguments().Single() : null;
    }

    public static Type? GetCollectionType(this Type type)
    {
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (CollectionTypes.Contains(genericType))
            {
                return genericType;
            }
        }

        return null;
    }

    public static bool HasInterfaceMethodOverride(this Type sourceType, Type interfaceType)
    {
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException("Parameter 'interfaceType' must be an interface type.");
        }

        if (!interfaceType.IsAssignableFrom(sourceType))
        {
            throw new ArgumentException("The 'sourceType' does not implement the 'interfaceType'.");
        }

        var map = sourceType.GetInterfaceMap(interfaceType);

        for (var i = 0; i < map.InterfaceMethods.Length; i++)
        {
            var interfaceMethod = map.InterfaceMethods[i];
            var targetMethod = map.TargetMethods[i];

            if (interfaceMethod != targetMethod)
            {
                return true;
            }
        }

        return false;
    }

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
        if (!interfaceType.IsInterface) throw new ArgumentException($"Type \"{interfaceType.Name}\" is not interface");

        var interfaces = type.GetAllInterfaces().ToArray();

        return interfaceType.IsGenericTypeDefinition
            ? interfaces.Any(i => i.IsGenericTypeImplementation(interfaceType, implementArguments))
            : interfaces.Contains(interfaceType);
    }

    public static IEnumerable<Type> GetAllInterfaces(this Type type, bool unwrapSubInterfaceGenerics = true)
    {
        return type.IsInterface
            ? new[] { type }.Concat(type.GetInterfaces().Pipe(type.IsGenericTypeDefinition && unwrapSubInterfaceGenerics,
                res => res.Select(subType => subType.IsGenericType ? subType.GetGenericTypeDefinition() : subType).ToArray()))
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

    public static MethodInfo? GetEqualityMethod(this Type type, bool withBaseTypes = false)
    {
        if (withBaseTypes)
        {
            return type.GetAllElements(t => t.BaseType).Select(t => t.GetEqualityMethod()).FirstOrDefault(t => t != null);
        }
        else
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m =>

                m.ReturnType == typeof(bool) && m.Name == "op_Equality"
                                             && m.GetParameters().Pipe(parameters =>

                                                 parameters.Length == 2 && parameters.All(parameter => parameter.ParameterType == type)));
        }
    }

    public static Type? GetMaybeElementType(this Type type)
    {
        return type.IsGenericTypeImplementation(typeof(Maybe<>)) ? type.GetGenericArguments().Single() : null;
    }

    public static bool IsMaybe(this Type type, bool withNested = false)
    {
        return type.GetMaybeElementType() != null

               || (withNested && (type.IsGenericTypeImplementation(typeof(Just<>)) || type.IsGenericTypeImplementation(typeof(Nothing<>))));
    }
}