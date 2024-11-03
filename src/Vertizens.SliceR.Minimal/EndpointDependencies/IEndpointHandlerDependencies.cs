namespace Vertizens.SliceR.Minimal;
internal interface IEndpointHandlerDependencies
{
    IEnumerable<Type> GetHandlerInterfaces();
}
