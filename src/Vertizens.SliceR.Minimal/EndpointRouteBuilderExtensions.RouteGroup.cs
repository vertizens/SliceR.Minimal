using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Vertizens.SliceR.Minimal;
public static partial class EndpointRouteBuilderExtensions
{

    public static RouteGroupBuilder MapRootRouteGroup<T>(this IEndpointRouteBuilder builder)
    {
        return builder.MapRootRouteGroup(typeof(T).Name);
    }

    public static RouteGroupBuilder MapRootRouteGroup(this IEndpointRouteBuilder builder, string routeGroupName)
    {
        var groupTag = routeGroupName;
        routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(routeGroupName);
        return builder.MapRouteGroup(routeGroupName, ApiRouteGroupPrefix.Api, groupTag).AddEndpointFilter<ValidatedResultEndpointFilter>();
    }

    public static RouteGroupBuilder MapRouteGroup<T>(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
    {
        var entityType = typeof(T);
        var routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(entityType.Name);

        return builder.MapRouteGroup(routeGroupName, groupPrefix, groupTag);
    }

    public static RouteGroupBuilder MapRouteGroup(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string routeGroupName, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
    {
        var prefix = groupPrefix != null ? string.Join('/', groupPrefix, routeGroupName) : routeGroupName;
        var routeGroup = builder.MapGroup(prefix);

        if (!string.IsNullOrWhiteSpace(groupTag))
        {
            routeGroup.WithTags(groupTag);
        }

        return routeGroup;
    }
}
