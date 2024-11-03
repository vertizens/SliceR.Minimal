namespace Vertizens.SliceR.Minimal;
internal interface IEndpointValidatedHandlerRegistrar
{
    bool Handle(Type validatedHandlerInterface, ValidatedHandlerRegistrarContext context);
}
