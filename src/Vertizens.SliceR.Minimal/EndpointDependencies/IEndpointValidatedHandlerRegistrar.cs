namespace Vertizens.SliceR.Minimal;
internal interface IEndpointValidatedHandlerRegistrar
{
    bool Handle(EndpointHandler endpointHandler, ValidatedHandlerRegistrarContext context);
}
