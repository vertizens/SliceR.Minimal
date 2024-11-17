using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Extensions for <see cref="IEndpointRouteBuilder"/> that uses <see cref="IValidatedHandler{TRequest}"/> or <see cref="IValidatedHandler{TRequest, TResult}"/>
/// </summary>
public static partial class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="NoFilter"/> 
    /// and expects <see cref="IQueryable{T}"/> of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomain">Expected domain type</typeparam>
    /// <param name="endpoints">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="pattern">any route pattern for building endpoint</param>
    public static RouteHandlerBuilder MapGetAsNoFilterQueryable<TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapGet(pattern, (IValidatedHandler<NoFilter, IQueryable<TDomain>> noFilterHandler) =>
            noFilterHandler.Handle(new NoFilter()))
            .Produces<IQueryable<TDomain>>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="ByKey{TId}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TId">Id or key value for TDomain, expects property called Id</typeparam>
    /// <typeparam name="TDomain">Expected domain type</typeparam>
    /// <param name="endpoints">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="pattern">any route pattern for building endpoint</param>
    /// <returns></returns>
    public static RouteHandlerBuilder MapGetAsById<TId, TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "{id}")
    {
        return endpoints.MapGet(pattern, (TId id, IValidatedHandler<ByKey<TId>, TDomain?> byKeyHandler) =>
            byKeyHandler.Handle(id))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <typeparamref name="TRequest"/>
    /// and expects an instance of type <typeparamref name="TResult"/> in response.
    /// </summary>
    public static RouteHandlerBuilder MapGet<TRequest, TResult>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    where TRequest : class, new()
    {
        return endpoints.MapGet(pattern, (IValidatedHandler<TRequest, TResult> getHandler) =>
            getHandler.Handle(new TRequest()))
            .Produces<TResult>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Insert{TDomainInsert}"/> 
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TDomainInsert">Type to map to entity</typeparam>
    /// <typeparam name="TDomain">Type that is the response</typeparam>
    /// <returns></returns>
    public static RouteHandlerBuilder MapPostAsInsert<TDomainInsert, TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapPost(pattern, (TDomainInsert domainInsert, IValidatedHandler<Insert<TDomainInsert>, TDomain?> insertHandler) =>
            insertHandler.Handle(new Insert<TDomainInsert>(domainInsert)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <typeparamref name="TRequest"/>
    /// and expects an instance of type <typeparamref name="TResult"/> in response.
    /// </summary>
    public static RouteHandlerBuilder MapPost<TRequest, TResult>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapPost(pattern, (TRequest request, IValidatedHandler<TRequest, TResult> postHandler) =>
            postHandler.Handle(request))
            .Produces<TResult>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <typeparamref name="TRequest"/>
    /// and expects no content in response.
    /// </summary>
    public static RouteHandlerBuilder MapPost<TRequest>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapPost(pattern, async (TRequest request, IValidatedHandler<TRequest> postHandlerNoResult) =>
            (await postHandlerNoResult.Handle(request)).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest, TResult}"/> that requests <see cref="Update{TId, TDomainUpdate}"/>
    /// and expects an instance of type <typeparamref name="TDomain"/> in response.
    /// </summary>
    /// <typeparam name="TId">Id property of existing domain instance</typeparam>
    /// <typeparam name="TDomainUpdate">Domain maps to existing entity to be updated</typeparam>
    /// <typeparam name="TDomain">Type to be returned</typeparam>
    public static RouteHandlerBuilder MapPutAsById<TId, TDomainUpdate, TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "{id}")
    {
        return endpoints.MapPut(pattern, (TId id, TDomainUpdate domainUpdate, IValidatedHandler<Update<TId, TDomainUpdate>, TDomain?> updateHandler) =>
            updateHandler.Handle(new Update<TId, TDomainUpdate>(id, domainUpdate)))
            .Produces<TDomain>();
    }

    /// <summary>
    /// Builds an endpoint for a <see cref="IValidatedHandler{TRequest}"/> that requests <see cref="Delete{TId, TDomain}"/>
    /// and expects no content in response.
    /// </summary>
    /// <typeparam name="TId">Id property of the domain to be deleted</typeparam>
    /// <typeparam name="TDomain">Type of domain that maps to an entity to be deleted</typeparam>
    public static RouteHandlerBuilder MapDeleteAsById<TId, TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "{id}")
    {
        return endpoints.MapDelete(pattern, async (TId id, IValidatedHandler<Delete<TId, TDomain>> deleteHandler) =>
            (await deleteHandler.Handle(new Delete<TId, TDomain>(id))).ToHttpResult())
            .Produces(StatusCodes.Status204NoContent);
    }
}
