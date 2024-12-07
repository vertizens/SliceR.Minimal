using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Vertizens.SliceR.Minimal;
public static partial class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="keySelector">func to select key property from a key type</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty> MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(this IEndpointRouteBuilder builder, Func<TKey, TKeyProperty> keySelector)
        where TKey : notnull
    {
        return builder.MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(typeof(TEntity).Name, keySelector);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Custom route group name</param>
    /// <param name="keySelector">func to select key property from a key type</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty> MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(this IEndpointRouteBuilder builder, string routeGroupName, Func<TKey, TKeyProperty> keySelector)
        where TKey : notnull
    {
        var groupTag = routeGroupName;
        routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(routeGroupName);
        return builder.MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(routeGroupName, keySelector, ApiRouteGroupPrefix.Api, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="keySelector">func to select key property from a key type</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty> MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(
        this IEndpointRouteBuilder builder,
        Func<TKey, TKeyProperty> keySelector,
        [StringSyntax("Route")] string? groupPrefix = null,
        string? groupTag = null
        ) where TKey : notnull
    {
        var entityType = typeof(TEntity);
        var routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(entityType.Name);

        return builder.MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(routeGroupName, keySelector, groupPrefix, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Custom route group name</param>
    /// <param name="keySelector">func to select key property from a key type</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty> MapEntityRouteGroup<TEntity, TKey, TKeyProperty>(
        this IEndpointRouteBuilder builder,
        [StringSyntax("Route")] string routeGroupName,
        Func<TKey, TKeyProperty> keySelector,
        [StringSyntax("Route")] string? groupPrefix = null,
        string? groupTag = null
        ) where TKey : notnull
    {
        var prefix = groupPrefix != null ? string.Join('/', groupPrefix, routeGroupName) : routeGroupName;
        var routeGroup = builder.MapGroup(prefix);

        if (!string.IsNullOrWhiteSpace(groupTag))
        {
            routeGroup.WithTags(groupTag);
        }

        routeGroup.AddEndpointFilter<ValidatedResultEndpointFilter>();

        return new EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty>(routeGroup, keySelector);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="keySelector">func to select key property from a key type</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain> MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(this IEndpointRouteBuilder builder, Func<TKey, TKeyProperty> keySelector)
    where TKey : notnull
    {
        return builder.MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(typeof(TEntity).Name, keySelector);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Custom route group name</param>
    /// <param name="keySelector">func to select key property from a key type</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain> MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(this IEndpointRouteBuilder builder, string routeGroupName, Func<TKey, TKeyProperty> keySelector)
        where TKey : notnull
    {
        var groupTag = routeGroupName;
        routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(routeGroupName);
        return builder.MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(routeGroupName, keySelector, ApiRouteGroupPrefix.Api, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="keySelector">func to select key property from a key type</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain> MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(
        this IEndpointRouteBuilder builder,
        Func<TKey, TKeyProperty> keySelector,
        [StringSyntax("Route")] string? groupPrefix = null,
        string? groupTag = null
        ) where TKey : notnull
    {
        var entityType = typeof(TEntity);
        var routeGroupName = JsonNamingPolicy.KebabCaseLower.ConvertName(entityType.Name);

        return builder.MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(routeGroupName, keySelector, groupPrefix, groupTag);
    }

    /// <summary>
    /// Creates <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/> with prefix and tag to build endpoints for.
    /// Used when there is a need to control the parameter name for the key instead of default 'id'.  Create a key type with a 
    /// property that matches the key and expect AsParameters attribute to be used with it.
    /// </summary>
    /// <typeparam name="TEntity">Entity with which to create a <see cref="EntityKeySelectorRouteGroupBuilder{TEntity,TKey,TKeyProperty,TDomain}"/>.</typeparam>
    /// <typeparam name="TKey">Key of the entity</typeparam>
    /// <typeparam name="TKeyProperty"></typeparam>
    /// <typeparam name="TDomain">Domain type that will be returned in place of entity</typeparam>
    /// <param name="builder">Instance of <see cref="IEndpointRouteBuilder"/></param>
    /// <param name="routeGroupName">Custom route group name</param>
    /// <param name="keySelector">func to select key property from a key type</param>
    /// <param name="groupPrefix">set a group prefix</param>
    /// <param name="groupTag">define the group tag</param>
    public static EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain> MapEntityRouteGroup<TEntity, TKey, TKeyProperty, TDomain>(
        this IEndpointRouteBuilder builder,
        [StringSyntax("Route")] string routeGroupName,
        Func<TKey, TKeyProperty> keySelector,
        [StringSyntax("Route")] string? groupPrefix = null,
        string? groupTag = null
        ) where TKey : notnull
    {
        var prefix = groupPrefix != null ? string.Join('/', groupPrefix, routeGroupName) : routeGroupName;
        var routeGroup = builder.MapGroup(prefix);

        if (!string.IsNullOrWhiteSpace(groupTag))
        {
            routeGroup.WithTags(groupTag);
        }

        routeGroup.AddEndpointFilter<ValidatedResultEndpointFilter>();

        return new EntityKeySelectorRouteGroupBuilder<TEntity, TKey, TKeyProperty, TDomain>(routeGroup, keySelector);
    }
}
