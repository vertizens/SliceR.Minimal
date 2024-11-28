using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class EndpointHandlerDependencies(IEnumerable<IEndpointBuilder> _endpointBuilders) : IEndpointHandlerDependencies
{
    public IEnumerable<EndpointHandler> GetEndpointHandlers()
    {
        var serviceProvider = new EmptyServiceProvider();
        var startupEndpointBuilder = new StartupEndpointRouteBuilder(serviceProvider);
        foreach (var endpointBuilder in _endpointBuilders)
        {
            endpointBuilder.Build(startupEndpointBuilder);
        }

        var endpointHandlers = new List<EndpointHandler>();
        foreach (var endpointDatasource in startupEndpointBuilder.DataSources)
        {
            foreach (var endpoint in endpointDatasource.Endpoints)
            {
                var handlerInterfaces = serviceProvider.ServiceValidator.GetHandlerInterfaces();
                if (handlerInterfaces.Count > 0)
                {
                    endpointHandlers.AddRange(handlerInterfaces.Select(x => new EndpointHandler(endpoint, x)));
                    serviceProvider.ServiceValidator.ClearHandlerInterfaces();
                }
            }
        }

        return endpointHandlers;
    }

    private class EmptyServiceProvider : IServiceProvider
    {
        public ServiceProviderIsService ServiceValidator { get; init; } = new();
        public object? GetService(Type serviceType)
        {
            return serviceType == typeof(IServiceProviderIsService) ? ServiceValidator : null;
        }
    }

    private class ServiceProviderIsService : IServiceProviderIsService
    {
        private Dictionary<Type, Type[]> _validatedHandlerTypes = [];

        public bool IsService(Type serviceType)
        {
            if (serviceType.IsGenericType && (serviceType.GetGenericTypeDefinition() == typeof(IValidatedHandler<,>)
                || serviceType.GetGenericTypeDefinition() == typeof(IValidatedHandler<>)))
            {
                _validatedHandlerTypes[serviceType] = serviceType.GetGenericArguments();
                return true;
            }

            return false;
        }

        public List<Type> GetHandlerInterfaces()
        {
            return [.. _validatedHandlerTypes.Keys];
        }

        public void ClearHandlerInterfaces()
        {
            _validatedHandlerTypes = [];
        }
    }

    private class StartupEndpointRouteBuilder(IServiceProvider _serviceProvider) : IEndpointRouteBuilder
    {
        private readonly IList<EndpointDataSource> _dataSources = [];
        public IServiceProvider ServiceProvider => _serviceProvider;

        public ICollection<EndpointDataSource> DataSources => _dataSources;

        public IApplicationBuilder CreateApplicationBuilder()
        {
            throw new NotImplementedException();
        }
    }
}
