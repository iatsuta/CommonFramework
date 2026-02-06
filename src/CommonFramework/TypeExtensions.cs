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

    extension(Type type)
    {
        public Type? GetNullableElementType()
        {
            return type.IsGenericTypeImplementation(typeof(Nullable<>)) ? type.GetGenericArguments().Single() : null;
        }

        public Type GetCollectionElementTypeOrSelf()
        {
            return type.GetCollectionElementType() ?? type;
        }

        public Type? GetCollectionElementType()
        {
            return type.GetCollectionType() != null ? type.GetGenericArguments().Single() : null;
        }

        public Type? GetCollectionType()
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

        public bool HasInterfaceMethodOverride(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Parameter 'interfaceType' must be an interface type.");
            }

            if (!interfaceType.IsAssignableFrom(type))
            {
                throw new ArgumentException("The 'sourceType' does not implement the 'interfaceType'.");
            }

            var map = type.GetInterfaceMap(interfaceType);

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

        public bool IsGenericTypeImplementation(Type genericTypeDefinition, Type[]? implementArguments = null)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition
                                      && (implementArguments == null || type.GetGenericArguments().SequenceEqual(implementArguments));
        }

        public Type? GetInterfaceImplementationArgument(Type interfaceType)
        {
            return type.GetInterfaceImplementationArguments(interfaceType, args => args.Single());
        }

        public TResult? GetInterfaceImplementationArguments<TResult>(Type interfaceType, Func<Type[], TResult> getResult)
            where TResult : class
        {
            return type.GetInterfaceImplementation(interfaceType).Maybe(i => getResult(i.GetGenericArguments()));
        }

        public Type? GetInterfaceImplementation(Type interfaceType)
        {
            return type.IsInterfaceImplementation(interfaceType)
                ? type.GetAllInterfaces().SingleOrDefault(i => IsGenericTypeImplementation(i, interfaceType))
                : null;
        }

        public bool IsInterfaceImplementation(Type interfaceType, Type[]? implementArguments = null)
        {
            if (!interfaceType.IsInterface) throw new ArgumentException($"Type \"{interfaceType.Name}\" is not interface");

            var interfaces = type.GetAllInterfaces().ToArray();

            return interfaceType.IsGenericTypeDefinition
                ? interfaces.Any(i => i.IsGenericTypeImplementation(interfaceType, implementArguments))
                : interfaces.Contains(interfaceType);
        }

        public IEnumerable<Type> GetAllInterfaces(bool unwrapSubInterfaceGenerics = true)
        {
            return type.IsInterface
                ? new[] { type }.Concat(type.GetInterfaces().Pipe(type.IsGenericTypeDefinition && unwrapSubInterfaceGenerics,
                    res => res.Select(subType => subType.IsGenericType ? subType.GetGenericTypeDefinition() : subType).ToArray()))
                : type.GetInterfaces();
        }

        public Type? GetGenericTypeImplementationArgument(Type genericTypeDefinition)
        {
            return type.GetGenericTypeImplementationArguments(genericTypeDefinition, args => args.Single());
        }

        public TResult? GetGenericTypeImplementationArguments<TResult>(Type genericTypeDefinition, Func<Type[], TResult> getResult)
            where TResult : class
        {
            return type.IsGenericTypeImplementation(genericTypeDefinition)
                ? getResult(type.GetGenericArguments())
                : null;
        }

        public MethodInfo? GetEqualityMethod(bool withBaseTypes = false)
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

        public Type? GetMaybeElementType()
        {
            return type.IsGenericTypeImplementation(typeof(Maybe<>)) ? type.GetGenericArguments().Single() : null;
        }

        public bool IsMaybe(bool withNested = false)
        {
            return type.GetMaybeElementType() != null

                   || (withNested && (type.IsGenericTypeImplementation(typeof(Just<>)) || type.IsGenericTypeImplementation(typeof(Nothing<>))));
        }

        public PropertyInfo GetRequiredProperty(string propertyName, BindingFlags bindingFlags)
        {
            return type.GetProperty(propertyName, bindingFlags) ?? throw new Exception($"{propertyName} property in {type.Name} not found");
        }
    }
}