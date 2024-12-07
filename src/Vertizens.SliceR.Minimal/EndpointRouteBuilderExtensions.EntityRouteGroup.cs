using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Vertizens.SliceR.Minimal;
public static partial class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/> with a given group name with default prefix of <see cref="ApiRouteGroupPrefix.Api"/>.
    /// Group tag matches name of type in 
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/></typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    public static EntityRouteGroupBuilder<TEntity, TKey> MapEntityRouteGroup<TEntity, TKey>(this IEndpointRouteBuilder builder)
        where TKey : notnull
    {
        return builder.MapEntityRouteGroup<TEntity, TKey>(typeof(TEntity).Name);
    }

    /// <summary>
    /// Creates RouteGroupBuilder with a given group name. Group tag matches group name.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/></typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Name of this group in kebab case</param>
    /// <returns></returns>
    public static EntityRouteGroupBuilder<TEntity, TKey> MapEntityRouteGroup<TEntity, TKey>(this IEndpointRouteBuilder builder, string routeGroupName)
        where TKey : notnull
    {
        var groupTag = routeGroupName;
        routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(routeGroupName);
        return builder.MapEntityRouteGroup<TEntity, TKey>(routeGroupName, ApiRouteGroupPrefix.Api, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/> with prefix and tag to build endpoints for.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/>. Defaults the name to the name of this type in kebab case</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    /// <returns></returns>
    public static EntityRouteGroupBuilder<TEntity, TKey> MapEntityRouteGroup<TEntity, TKey>(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
        where TKey : notnull
    {
        var entityType = typeof(TEntity);
        var routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(entityType.Name);

        return builder.MapEntityRouteGroup<TEntity, TKey>(routeGroupName, groupPrefix, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/> with specific custom group name, prefix, and tag.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey}"/></typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">sets the route url with group name</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    /// <returns></returns>
    public static EntityRouteGroupBuilder<TEntity, TKey> MapEntityRouteGroup<TEntity, TKey>(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string routeGroupName, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
        where TKey : notnull
    {
        var prefix = groupPrefix != null ? string.Join('/', groupPrefix, routeGroupName) : routeGroupName;
        var routeGroup = builder.MapGroup(prefix);

        if (!string.IsNullOrWhiteSpace(groupTag))
        {
            routeGroup.WithTags(groupTag);
        }

        routeGroup.AddEndpointFilter<ValidatedResultEndpointFilter>();

        return routeGroup;
    }

    /// <summary>
    /// Creates RouteGroupBuilder with a given group name with default prefix of <see cref="ApiRouteGroupPrefix.Api"/>.
    /// Group tag matches name of type in 
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/></typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    public static EntityRouteGroupBuilder<TEntity, TKey, TDomain> MapEntityRouteGroup<TEntity, TKey, TDomain>(this IEndpointRouteBuilder builder)
        where TKey : notnull
    {
        return builder.MapEntityRouteGroup<TEntity, TKey, TDomain>(typeof(TEntity).Name);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/> with a given group name. Group tag matches group name.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/></typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Name of this group in kebab case</param>
    /// <returns></returns>
    public static EntityRouteGroupBuilder<TEntity, TKey, TDomain> MapEntityRouteGroup<TEntity, TKey, TDomain>(this IEndpointRouteBuilder builder, string routeGroupName)
        where TKey : notnull
    {
        var groupTag = routeGroupName;
        routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(routeGroupName);
        return builder.MapEntityRouteGroup<TEntity, TKey, TDomain>(routeGroupName, ApiRouteGroupPrefix.Api, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/> with prefix and tag to build endpoints for.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/>. Defaults the name to the name of this type in kebab case</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    /// <returns></returns>
    public static EntityRouteGroupBuilder<TEntity, TKey, TDomain> MapEntityRouteGroup<TEntity, TKey, TDomain>(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
        where TKey : notnull
    {
        var entityType = typeof(TEntity);
        var routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(entityType.Name);

        return builder.MapEntityRouteGroup<TEntity, TKey, TDomain>(routeGroupName, groupPrefix, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/> with prefix and tag to build endpoints for.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityRouteGroupBuilder{TEntity,TKey,TDomain}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Custom route group name</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    /// <returns></returns>
    public static EntityRouteGroupBuilder<TEntity, TKey, TDomain> MapEntityRouteGroup<TEntity, TKey, TDomain>(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string routeGroupName, [StringSyntax("Route")] string? groupPrefix = null, string? groupTag = null)
        where TKey : notnull
    {
        var prefix = groupPrefix != null ? string.Join('/', groupPrefix, routeGroupName) : routeGroupName;
        var routeGroup = builder.MapGroup(prefix);

        if (!string.IsNullOrWhiteSpace(groupTag))
        {
            routeGroup.WithTags(groupTag);
        }

        routeGroup.AddEndpointFilter<ValidatedResultEndpointFilter>();

        return routeGroup;
    }
}
