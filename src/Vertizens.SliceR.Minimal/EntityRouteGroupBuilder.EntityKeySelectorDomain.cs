using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Entity/Key/KeySelector/Domain specific endpoint builder.  Provides helpful methods for common entity operations.
/// Used for when key type is created to use with AsParameters attribute.  KeySelector is used for selecting a property from key type.
/// Used to be able to customize the key parameter name in api documention and route pattern.
/// </summary>
/// <typeparam name="TKey">key value type for TEntity</typeparam>
/// <typeparam name="TEntity">Expected entity type</typeparam>
/// <typeparam name="TKeyProperty">Key selector function for getting property from key</typeparam>
/// <typeparam name="TDomain">Domain to map/project entity to from a response</typeparam>
public class EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain>(
    RouteGroupBuilder builder,
    Func<TKey, TKeyProperty> _keySelector
    ) : EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty>(builder, _keySelector)
    where TKey : notnull
{

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TKey}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public override RouteHandlerBuilder MapGetAsByKey([StringSyntax("Route")] string pattern)
    {
        return MapGetAsByKey<TDomain>(pattern);
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TKey, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapPutAsUpdateByKey<TDomainUpdate>([StringSyntax("Route")] string pattern)
    {
        return MapPutAsUpdateByKey<TDomainUpdate, TDomain>(pattern);
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TKey, TDomain}"/>
    /// and expects no content in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapDeleteAsByKey([StringSyntax("Route")] string pattern)
    {
        return MapDeleteAsByKey<TDomain>(pattern);
    }

    /// <summary>
    /// Gets a <see cref="RouteGroupBuilder"/> from a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity, TKey, TKeyProperty, TDomain}"/>
    /// </summary>
    /// <param name="entityRouteGroupBuilder">instance of <see cref="EntityKeySelectorRouteGroupBuilder{TEntity, TKey, TKeyProperty, TDomain}"/></param>
    public static implicit operator RouteGroupBuilder(EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain> entityRouteGroupBuilder) => entityRouteGroupBuilder.Builder;
}