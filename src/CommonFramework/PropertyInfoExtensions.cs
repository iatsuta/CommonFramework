using CommonFramework.DictionaryCache;

using System.Linq.Expressions;
using System.Reflection;

namespace CommonFramework;

public static class PropertyInfoExtensions
{
    public static Expression<Func<TSource, TProperty>> ToGetLambdaExpression<TSource, TProperty>(this PropertyInfo property)
    {
        return PropertyLambdaCache<TSource, TProperty>.GetLambdaCache[property];
    }

    public static Expression<Action<TSource, TProperty>> ToSetLambdaExpression<TSource, TProperty>(this PropertyInfo property)
    {
        return PropertyLambdaCache<TSource, TProperty>.SetLambdaCache[property];
    }

    public static LambdaExpression ToGetLambdaExpression(this PropertyInfo property, Type? sourceType = null)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return PropertyLambdaCache.GetLambdaCache!.GetValue(property, sourceType ?? property.ReflectedType);
    }

    public static LambdaExpression ToSetLambdaExpression(this PropertyInfo property, Type? sourceType = null)
    {
        return PropertyLambdaCache.SetLambdaCache!.GetValue(property, sourceType ?? property.ReflectedType);
    }

    public static LambdaExpression ToLambdaExpression(this PropertyInfo property, Type? sourceType = null)
    {
        return PropertyLambdaCache.GetLambdaCache!.GetValue(property, sourceType ?? property.ReflectedType);
    }

    public static Func<TSource, TProperty> GetGetValueFunc<TSource, TProperty>(this PropertyInfo property)
    {
        return PropertyLambdaCache<TSource, TProperty>.GetFuncCache[property];
    }

    public static Action<TSource, TProperty> GetSetValueAction<TSource, TProperty>(this PropertyInfo property)
    {
        return PropertyLambdaCache<TSource, TProperty>.SetActionCache[property];
    }

    private static class PropertyLambdaCache
    {
        public static readonly IDictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression> GetLambdaCache =
            new DictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression>(tuple =>
            {
                var property = tuple.Item1;
                var propertySource = tuple.Item2;

                var parameter = Expression.Parameter(propertySource);

                return Expression.Lambda(Expression.Property(parameter, property), parameter);
            }).WithLock();

        public static readonly IDictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression> SetLambdaCache =
            new DictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression>(tuple =>
            {
                var property = tuple.Item1;
                var propertySource = tuple.Item2;

                var sourceParameter = Expression.Parameter(propertySource);
                var valueParameter = Expression.Parameter(property.PropertyType);

                return Expression.Lambda(Expression.Call(sourceParameter, property.GetSetMethod() ?? property.GetSetMethod(true)!, valueParameter), sourceParameter,
                    valueParameter);
            }).WithLock();
    }

    private static class PropertyLambdaCache<TSource, TProperty>
    {
        public static readonly IDictionaryCache<PropertyInfo, Expression<Func<TSource, TProperty>>> GetLambdaCache =
            new DictionaryCache<PropertyInfo, Expression<Func<TSource, TProperty>>>(property =>
                (Expression<Func<TSource, TProperty>>)property.ToLambdaExpression(typeof(TSource))).WithLock();

        public static readonly IDictionaryCache<PropertyInfo, Func<TSource, TProperty>> GetFuncCache = new DictionaryCache<PropertyInfo, Func<TSource, TProperty>>(property =>
            GetLambdaCache[property].Compile()).WithLock();


        public static readonly IDictionaryCache<PropertyInfo, Expression<Action<TSource, TProperty>>> SetLambdaCache =
            new DictionaryCache<PropertyInfo, Expression<Action<TSource, TProperty>>>(property =>
                (Expression<Action<TSource, TProperty>>)property.ToSetLambdaExpression(typeof(TSource))).WithLock();

        public static readonly IDictionaryCache<PropertyInfo, Action<TSource, TProperty>> SetActionCache = new DictionaryCache<PropertyInfo, Action<TSource, TProperty>>(property =>
            SetLambdaCache[property].Compile()).WithLock();
    }
}