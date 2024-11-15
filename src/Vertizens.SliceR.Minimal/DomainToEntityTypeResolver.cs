namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Defines a Domain to Entity mapping strategy that looks for <see cref="IDomainToEntity{TEntity}"/> indicator interface 
/// on the domain type that points to the entity it will be mapped to
/// </summary>
internal class DomainToEntityTypeResolver : IDomainToEntityTypeResolver
{
    public Type? GetEntityType(Type domainType)
    {
        Type? entityType = null;
        var domainMaps = domainType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainToEntity<>)).ToList();
        if (domainMaps.Count == 1)
        {
            entityType = domainMaps[0].GetGenericArguments()[0];
        }
        return entityType;
    }
}
