namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Metadata instance to put on endpoint to indicate it is related to an entity
/// </summary>
/// <typeparam name="TEntity">Entity this domain/endpoint is linked to</typeparam>
public class EntityMetadata<TEntity> : IEntityMetadata<TEntity>
{
}
