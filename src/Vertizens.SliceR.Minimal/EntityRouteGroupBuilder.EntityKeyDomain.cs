using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Entity/Key/Domain specific endpoint builder.  Provides helpful methods for common entity operations.
/// </summary>
/// <typeparam name="TKey">Id or key value for TEntity</typeparam>
/// <typeparam name="TEntity">Expected entity type</typeparam>
/// <typeparam name="TDomain">Domain to map/project entity to from a response</typeparam>
public class EntityRouteGroupBuilder<TEntity, TKey, TDomain>(RouteGroupBuilder builder) : EntityRouteGroupBuilder<TEntity, TKey>(builder)
{
    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="NoFilter"/> 
    /// and expects <see cref="IQueryable{T}"/> of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public override RouteHandlerBuilder MapGetAsNoFilterQueryable([StringSyntax("Route")] string pattern = "")
    {
        return Builder.MapGet(pattern, (IValidatedHandler<NoFilter, IQueryable<TDomain>> noFilterHandler) =>
            noFilterHandler.Handle(new NoFilter()))
            .Produces<IQueryable<TDomain>>().WithMetadata();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TKey}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public override RouteHandlerBuilder MapGetAsById([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapGet(pattern, (TKey id, IValidatedHandler<ByKey<TKey>, TDomain?> byKeyHandler) =>
            byKeyHandler.Handle(id))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Insert{TDomainInsert}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainInsert">Type to map to entity</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public override RouteHandlerBuilder MapPostAsInsert<TDomainInsert>([StringSyntax("Route")] string pattern = "")
    {
        return Builder.MapPost(pattern, (TDomainInsert domainInsert, IValidatedHandler<Insert<TDomainInsert>, TDomain?> insertHandler) =>
            insertHandler.Handle(new Insert<TDomainInsert>(domainInsert)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TKey, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapPutAsUpdateById<TDomainUpdate>([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapPut(pattern, (TKey id, TDomainUpdate domainUpdate, IValidatedHandler<Update<TKey, TDomainUpdate>, TDomain?> updateHandler) =>
            updateHandler.Handle(new Update<TKey, TDomainUpdate>(id, domainUpdate)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TKey, TDomain}"/>
    /// and expects no content in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public override RouteHandlerBuilder MapDeleteAsById([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapDelete(pattern, async (TKey id, IValidatedHandler<Delete<TKey, TDomain>> deleteHandler) =>
            (await deleteHandler.Handle(new Delete<TKey, TDomain>(id))).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity, TKey, TDomain}"/> from a <see cref="RouteGroupBuilder"/>
    /// </summary>
    /// <param name="builder">instance of <see cref="RouteGroupBuilder"/></param>
    public static implicit operator EntityRouteGroupBuilder<TEntity, TKey, TDomain>(RouteGroupBuilder builder) => new(builder);

    /// <summary>
    /// Gets a <see cref="RouteGroupBuilder"/> from a <see cref="EntityRouteGroupBuilder{TEntity, TKey, TDomain}"/>
    /// </summary>
    /// <param name="entityRouteGroupBuilder">instance of <see cref="EntityRouteGroupBuilder{TEntity, TKey, TDomain}"/></param>
    public static implicit operator RouteGroupBuilder(EntityRouteGroupBuilder<TEntity, TKey, TDomain> entityRouteGroupBuilder) => entityRouteGroupBuilder.Builder;
}