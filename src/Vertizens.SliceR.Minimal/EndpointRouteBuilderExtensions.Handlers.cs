using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;

public static partial class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGetAsNoFilterQueryable<TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapGet(pattern, (IValidatedHandler<NoFilter, IQueryable<TDomain>> noFilterHandler) =>
            noFilterHandler.Handle(new NoFilter()))
            .Produces<IQueryable<TDomain>>();
    }

    public static RouteHandlerBuilder MapGetAsById<TDomain, TId>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "{id}")
    {
        return endpoints.MapGet(pattern, (TId id, IValidatedHandler<ByKey<TId>, TDomain?> byKeyHandler) =>
            byKeyHandler.Handle(id))
            .Produces<TDomain>();
    }

    public static RouteHandlerBuilder MapGet<TRequest, TResult>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    where TRequest : class, new()
    {
        return endpoints.MapGet(pattern, (IValidatedHandler<TRequest, TResult> getHandler) =>
            getHandler.Handle(new TRequest()))
            .Produces<TResult>();
    }

    public static RouteHandlerBuilder MapPostAsInsert<TDomainInsert, TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapPost(pattern, (TDomainInsert domainInsert, IValidatedHandler<Insert<TDomainInsert>, TDomain?> insertHandler) =>
            insertHandler.Handle(new Insert<TDomainInsert>(domainInsert)))
            .Produces<TDomain>();
    }

    public static RouteHandlerBuilder MapPost<TRequest, TResult>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapPost(pattern, (TRequest request, IValidatedHandler<TRequest, TResult> postHandler) =>
            postHandler.Handle(request))
            .Produces<TResult>();
    }

    public static RouteHandlerBuilder MapPost<TRequest>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapPost(pattern, (TRequest request, IValidatedHandler<TRequest> postHandlerNoResult) =>
            postHandlerNoResult.Handle(request))
            .Produces(StatusCodes.Status204NoContent);
    }

    public static RouteHandlerBuilder MapPutAsById<TDomainUpdate, TId, TDomain>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "{id}")
    {
        return endpoints.MapPut(pattern, (TId id, TDomainUpdate domainUpdate, IValidatedHandler<Update<TId, TDomainUpdate>, TDomain?> updateHandler) =>
            updateHandler.Handle(new Update<TId, TDomainUpdate>(id, domainUpdate)))
            .Produces<TDomain>();
    }

    public static RouteHandlerBuilder MapDeleteAsById<TDomain, TId>(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "{id}")
    {
        return endpoints.MapDelete(pattern, (TId id, IValidatedHandler<Delete<TId, TDomain>> deleteHandler) =>
            deleteHandler.Handle(new Delete<TId, TDomain>(id)))
            .Produces(StatusCodes.Status204NoContent);
    }
}
