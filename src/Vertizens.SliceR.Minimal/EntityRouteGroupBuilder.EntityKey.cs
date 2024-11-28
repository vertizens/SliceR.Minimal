using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Entity/Key specific endpoint builder.  Provides helpful methods for common entity operations.
/// </summary>
/// <typeparam name="TKey">Id or key value for TEntity</typeparam>
/// <typeparam name="TEntity">Expected entity type</typeparam>
public class EntityRouteGroupBuilder<TEntity, TKey>
{
    /// <summary>
    /// Created from a <see cref="RouteGroupBuilder"/>
    /// </summary>
    /// <param name="builder">RouteGroupBuilder from <see cref="EndpointRouteBuilderExtensions"/></param> MapGroup method
    public EntityRouteGroupBuilder(RouteGroupBuilder builder)
    {
        Builder = builder;
        Builder.WithMetadata(new EntityMetadata<TEntity>());
    }

    /// <summary>
    /// Contained <see cref="RouteGroupBuilder"/>
    /// </summary>
    public RouteGroupBuilder Builder { get; }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="NoFilter"/> 
    /// and expects <see cref="IQueryable{T}"/> of type <typeparamref name="TEntity"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public virtual RouteHandlerBuilder MapGetAsNoFilterQueryable([StringSyntax("Route")] string pattern = "")
    {
        return Builder.MapGet(pattern, (IValidatedHandler<NoFilter, IQueryable<TEntity>> noFilterHandler) =>
            noFilterHandler.Handle(new NoFilter()))
            .Produces<IQueryable<TEntity>>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="NoFilter"/> 
    /// and expects <see cref="IQueryable{T}"/> of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public RouteHandlerBuilder MapGetAsNoFilterQueryable<TDomain>([StringSyntax("Route")] string pattern = "")
    {
        return Builder.MapGet(pattern, (IValidatedHandler<NoFilter, IQueryable<TDomain>> noFilterHandler) =>
            noFilterHandler.Handle(new NoFilter()))
            .Produces<IQueryable<TDomain>>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TKey}"/> 
    /// and expects an instance of type <typeparamref name="TEntity"/> in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public virtual RouteHandlerBuilder MapGetAsById([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapGet(pattern, (TKey id, IValidatedHandler<ByKey<TKey>, TEntity?> byKeyHandler) =>
            byKeyHandler.Handle(id))
            .Produces<TEntity>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TKey}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomain">Expected entity type</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public RouteHandlerBuilder MapGetAsById<TDomain>([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapGet(pattern, (TKey id, IValidatedHandler<ByKey<TKey>, TDomain?> byKeyHandler) =>
            byKeyHandler.Handle(id))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Insert{TDomainInsert}"/> 
    /// and expects an instance of type <typeparamref name="TEntity"/> in response.
    /// </summary>
    /// <typeparam name="TDomainInsert">Type to map to entity</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public virtual RouteHandlerBuilder MapPostAsInsert<TDomainInsert>([StringSyntax("Route")] string pattern = "")
    {
        return Builder.MapPost(pattern, (TDomainInsert domainInsert, IValidatedHandler<Insert<TDomainInsert>, TEntity?> insertHandler) =>
            insertHandler.Handle(new Insert<TDomainInsert>(domainInsert)))
            .Produces<TEntity>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Insert{TDomainInsert}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainInsert">Type to map to entity</typeparam>
    /// <typeparam name="TDomain"></typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public RouteHandlerBuilder MapPostAsInsert<TDomainInsert, TDomain>([StringSyntax("Route")] string pattern = "")
    {
        return Builder.MapPost(pattern, (TDomainInsert domainInsert, IValidatedHandler<Insert<TDomainInsert>, TDomain?> insertHandler) =>
            insertHandler.Handle(new Insert<TDomainInsert>(domainInsert)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TKey, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TEntity"/> in response.
    /// </summary>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public virtual RouteHandlerBuilder MapPutAsUpdateById<TDomainUpdate>([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapPut(pattern, (TKey id, TDomainUpdate domainUpdate, IValidatedHandler<Update<TKey, TDomainUpdate>, TEntity?> updateHandler) =>
            updateHandler.Handle(new Update<TKey, TDomainUpdate>(id, domainUpdate)))
            .Produces<TEntity>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TKey, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <typeparam name="TDomain"></typeparam>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public RouteHandlerBuilder MapPutAsUpdateById<TDomainUpdate, TDomain>([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapPut(pattern, (TKey id, TDomainUpdate domainUpdate, IValidatedHandler<Update<TKey, TDomainUpdate>, TDomain?> updateHandler) =>
            updateHandler.Handle(new Update<TKey, TDomainUpdate>(id, domainUpdate)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TKey, TEntity}"/>
    /// and expects no content in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public virtual RouteHandlerBuilder MapDeleteAsById([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapDelete(pattern, async (TKey id, IValidatedHandler<Delete<TKey, TEntity>> deleteHandler) =>
            (await deleteHandler.Handle(new Delete<TKey, TEntity>(id))).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TKey, TDomain}"/>
    /// and expects no content in response.
    /// </summary>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public RouteHandlerBuilder MapDeleteAsById<TDomain>([StringSyntax("Route")] string pattern = "{id}")
    {
        return Builder.MapDelete(pattern, async (TKey id, IValidatedHandler<Delete<TKey, TDomain>> deleteHandler) =>
            (await deleteHandler.Handle(new Delete<TKey, TDomain>(id))).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity, TKey}"/> from a <see cref="RouteGroupBuilder"/>
    /// </summary>
    /// <param name="builder">instance of <see cref="RouteGroupBuilder"/></param>
    public static implicit operator EntityRouteGroupBuilder<TEntity, TKey>(RouteGroupBuilder builder) => new(builder);

    /// <summary>
    /// Gets a <see cref="RouteGroupBuilder"/> from a <see cref="EntityRouteGroupBuilder{TEntity, TKey}"/>
    /// </summary>
    /// <param name="entityRouteGroupBuilder">instance of <see cref="EntityRouteGroupBuilder{TEntity, TKey}"/></param>
    public static implicit operator RouteGroupBuilder(EntityRouteGroupBuilder<TEntity, TKey> entityRouteGroupBuilder) => entityRouteGroupBuilder.Builder;
}