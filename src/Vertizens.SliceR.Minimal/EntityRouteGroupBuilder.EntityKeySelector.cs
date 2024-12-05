using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
public class EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty>(
    RouteGroupBuilder builder,
    Func<TKey, TKeyProperty> _keySelector
    ) : EntityRouteGroupBuilder<TEntity, TKey>(builder)
    where TKey : notnull
{
    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TKey}"/> 
    /// and expects an instance of type <typeparamref name="TEntity"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapGetAsByKey([StringSyntax("Route")] string pattern)
    {
        return Builder.MapGet(pattern, ([AsParameters] TKey key, IValidatedHandler<ByKey<TKeyProperty>, TEntity?> byKeyHandler) =>
            byKeyHandler.Handle(_keySelector(key)))
            .Produces<TEntity>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TKey}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapGetAsByKey<TDomain>([StringSyntax("Route")] string pattern)
    {
        return Builder.MapGet(pattern, ([AsParameters] TKey key, IValidatedHandler<ByKey<TKeyProperty>, TDomain?> byKeyHandler) =>
            byKeyHandler.Handle(_keySelector(key)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TKey, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TEntity"/> in response.
    /// </summary>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapPutAsUpdateByKey<TDomainUpdate>([StringSyntax("Route")] string pattern)
    {
        return Builder.MapPut(pattern, ([AsParameters] TKey key, TDomainUpdate domainUpdate, IValidatedHandler<Update<TKeyProperty, TDomainUpdate>, TEntity?> updateHandler) =>
            updateHandler.Handle(new Update<TKeyProperty, TDomainUpdate>(_keySelector(key), domainUpdate)))
            .Produces<TEntity>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TKey, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <typeparam name="TDomain">Domain Type to return</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapPutAsUpdateByKey<TDomainUpdate, TDomain>([StringSyntax("Route")] string pattern)
    {
        return Builder.MapPut(pattern, ([AsParameters] TKey key, TDomainUpdate domainUpdate, IValidatedHandler<Update<TKeyProperty, TDomainUpdate>, TEntity?> updateHandler) =>
            updateHandler.Handle(new Update<TKeyProperty, TDomainUpdate>(_keySelector(key), domainUpdate)))
            .Produces<TEntity>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TKey, TEntity}"/>
    /// and expects no content in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapDeleteAsByKey([StringSyntax("Route")] string pattern)
    {
        return Builder.MapDelete(pattern, async ([AsParameters] TKey key, IValidatedHandler<Delete<TKeyProperty, TEntity>> deleteHandler) =>
            (await deleteHandler.Handle(new Delete<TKeyProperty, TEntity>(_keySelector(key)))).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TKey, TDomain}"/>
    /// and expects no content in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapDeleteAsByKey<TDomain>([StringSyntax("Route")] string pattern)
    {
        return Builder.MapDelete(pattern, async ([AsParameters] TKey key, IValidatedHandler<Delete<TKeyProperty, TDomain>> deleteHandler) =>
            (await deleteHandler.Handle(new Delete<TKeyProperty, TDomain>(_keySelector(key)))).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Gets a <see cref="RouteGroupBuilder"/> from a <see cref="EntityRouteGroupBuilder{TEntity, TKey, TDomain}"/>
    /// </summary>
    /// <param name="entityRouteGroupBuilder">instance of <see cref="EntityRouteGroupBuilder{TEntity, TKey, TDomain}"/></param>
    public static implicit operator RouteGroupBuilder(EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty> entityRouteGroupBuilder) => entityRouteGroupBuilder.Builder;
}