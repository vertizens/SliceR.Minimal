using Microsoft.Extensions.DependencyInjection;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Minimal;
internal class ValidatedHandlerRegistrarContext
{
    public required IServiceCollection Services { get; init; }
    public required IEntityMetadataTypeResolver EntityMetadataTypeResolver { get; init; }
    public required IEntityDefinitionResolver EntityDefinitionResolver { get; init; }
    public required IEntityDomainHandlerRegistrar EntityDomainHandlerRegistrar { get; init; }

}
