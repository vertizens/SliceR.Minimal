using Microsoft.AspNetCore.Http;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Defines a entity mapping strategy that looks for <see cref="IEntityMetadata{TEntity}"/> indicator interface 
/// on the domain type that points to the entity it will be mapped to
/// </summary>
internal class EntityMetadataTypeResolver : IEntityMetadataTypeResolver
{
    public Type? GetEntityType(Type domainType)
    {
        Type? entityType = null;
        var domainMaps = domainType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityMetadata<>)).ToList();
        if (domainMaps.Count == 1)
        {
            entityType = domainMaps[0].GetGenericArguments()[0];
        }
        return entityType;
    }

    public Type? GetEntityType(Endpoint endpoint)
    {
        Type? entityType = null;
        var entityMetadata = endpoint.Metadata.SelectMany(x =>
            x.GetType().GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityMetadata<>))).FirstOrDefault();

        if (entityMetadata != null)
        {
            entityType = entityMetadata.GetGenericArguments().First();
        }

        return entityType;
    }
}
