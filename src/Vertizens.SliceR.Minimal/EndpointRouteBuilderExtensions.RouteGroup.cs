using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Vertizens.SliceR.Minimal;
public static partial class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Creates RouteGroupBuilder with a given group name with default prefix of <see cref="ApiRouteGroupPrefix.Api"/>.
    /// Group tag matches name of type in 
    /// </summary>
    /// <typeparam name="T">Defaults the name to the name of this type in kebab case</typeparam>
    public static RouteGroupBuilder MapRootRouteGroup<T>(this IEndpointRouteBuilder builder)
    {
        return builder.MapRootRouteGroup(typeof(T).Name);
    }

    /// <summary>
    /// Creates RouteGroupBuilder with a given group name. Group tag matches group name.
    /// </summary>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Name of this group in kebab case</param>
    /// <returns></returns>
    public static RouteGroupBuilder MapRootRouteGroup(this IEndpointRouteBuilder builder, string routeGroupName)
    {
        var groupTag = routeGroupName;
        routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(routeGroupName);
        return builder.MapRouteGroup(routeGroupName, ApiRouteGroupPrefix.Api, groupTag).AddEndpointFilter<ValidatedResultEndpointFilter>();
    }

    /// <summary>
    /// Creates RouteGroupBuilder with prefix and tag to build endpoints for.  Can be used to make a group within a group.
    /// </summary>
    /// <typeparam name="T">Defaults the name to the name of this type in kebab case</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    /// <returns></returns>
    public static RouteGroupBuilder MapRouteGroup<T>(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
    {
        var entityType = typeof(T);
        var routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(entityType.Name);

        return builder.MapRouteGroup(routeGroupName, groupPrefix, groupTag);
    }

    /// <summary>
    /// Creates RouteGroupBuilder with specific custom group name, prefix, and tag.
    /// </summary>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">sets the route url with group name</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    /// <returns></returns>
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
