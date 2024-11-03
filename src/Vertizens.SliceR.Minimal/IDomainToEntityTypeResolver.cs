namespace Vertizens.SliceR.Minimal;
public interface IDomainToEntityTypeResolver
{
    Type? GetEntityType(Type domainType);
}
