using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Text;
using Vertizens.ServiceProxy;
using Vertizens.SliceR.Operations;
using Vertizens.TypeMapper;

namespace Vertizens.SliceR.Minimal;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddSliceREndpointDefaultValidatedHandlers(this IServiceCollection services)
    {
        services.AddTypeMappers();
        services.TryAddSingleton<IDomainToEntityTypeResolver, DomainToEntityTypeResolver>();
        services.AddInterfaceTypes<IEndpointValidatedHandlerRegistrar>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var context = new ValidatedHandlerRegistrarContext
        {
            Services = services,
            EntityDefinitionResolver = serviceProvider.GetRequiredService<IEntityDefinitionResolver>(),
            DomainToEntityTypeResolver = serviceProvider.GetRequiredService<IDomainToEntityTypeResolver>(),
            EntityDomainHandlerRegistrar = serviceProvider.GetRequiredService<IEntityDomainHandlerRegistrar>()
        };
        var endpointDependencies = (IEndpointHandlerDependencies)ActivatorUtilities.CreateInstance(serviceProvider, typeof(EndpointHandlerDependencies));
        var handlerInterfaces = endpointDependencies.GetHandlerInterfaces();

        var registrars = serviceProvider.GetServices<IEndpointValidatedHandlerRegistrar>();
        var isService = serviceProvider.GetRequiredService<IServiceProviderIsService>();
        var unhandledTypes = new List<Type>();
        foreach (var handlerInterface in handlerInterfaces)
        {
            if (!isService.IsService(handlerInterface))
            {
                var handled = false;
                foreach (var registrar in registrars)
                {
                    if (registrar.Handle(handlerInterface, context))
                    {
                        handled = true;
                        break;
                    }
                }

                if (!handled)
                {
                    unhandledTypes.Add(handlerInterface);
                }
            }
        }

        if (unhandledTypes.Count > 0)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            var logger = loggerFactory.CreateLogger("Startup");

            var message = new StringBuilder();
            message.AppendLine("These handler types could not be default created:");
            unhandledTypes.ForEach(x => message.AppendLine(x.ToString()));

            logger.LogWarning(message.ToString());
        }

        return services;
    }
}
