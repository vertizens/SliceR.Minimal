# SliceR.Minimal

Adds Minimal API patterns for quickly adding endpoints to SliceR

## Getting Started

This code extends the .NET Minimal API codebase with `IEndpointBuilder`.  The idea is to define one or more endpoints in a set of classes that can be registered with DI. 

register all your `IEndpointBuilder` classes in your assembly with

    services.AddEndpointBuilders();

In program.cs make sure to use them in your WebApplication by calling `AddEndpointBuilders`

    var builder = WebApplication.CreateBuilder(args);

    //... more code

    var app = builder.Build();

    //... more code

    app.AddEndpointBuilders();
    
    app.Run();

## IEndpointBuilder

There are several helper extensions:  
`MapRootRouteGroup`

    public IEndpointRouteBuilder Build(IEndpointRouteBuilder builder)
    {
        var rootRouteBuilder = builder.MapRootRouteGroup<EntityType>();

        return builder;
    }

This defines a Builder to add endpoints all with the same prefix.  Overloads for group name, tag, and prefix.

Set `ApiRouteGroupPrefix.Api` to whatever constant you prefer on startup before building endpoints.

If you use standard Operations there are Map methods to simplify defining endpoints.  
Examples are:  
`MapGetAsNoFilterQueryable<TDomain>`  
`MapGetAsById<TDomain, TId>`  
`MapGet<TRequest, TResult>`  
`MapPostAsInsert<TDomainInsert, TDomain>`  
`MapPost<TRequest, TResult>`  
`MapPost<TRequest>`  
`MapPutAsById<TDomainUpdate, TId, TDomain>`  
`MapDeleteAsById<TDomain, TId>`  

If using entities then use `MapEntityRouteGroup`

    public IEndpointRouteBuilder Build(IEndpointRouteBuilder builder)
    {
        var entityRouteBuilder = builder.MapEntityRouteGroup<EntityType, KeyType>();

        return builder;
    }
 
This returns `EntityRouteGroupBuilder<TEntity, TKey>` or `EntityRouteGroupBuilder<TEntity, TKey, TDomain>`
depending on the overload called. 

These have the same entity operation methods except now the Entity, Key, and Domain doesn't have to be repeated 
for each route.

For custom keys that select one property from a type then use `EntityKeySelectorRouteGroupBuilder` by calling `MapEntityRouteGroup<TEntity, TKey, TKeyProperty>`
Any ..AsKey method uses the AsParameters attribute on the key in the minimal delegate so make sure the route pattern given has 
the property names from the key type in use.

Any EntityRouteGroupBuilder puts the `EntityMetadata<TEntity>` on the endpoint so when figuring out handlers that use a domain for a 
response, the default registered handler is one that know what entity to use for querying.

## ValidatedResult

If you use `MapRootRouteGroup` then the `ValidatedResultEndpointFilter` is added by default.  This simply turns a `ValidatedResult` into an `IResult` for the endpoint.
  This means the result gets translated to what the Http result required so if you do custom things make sure you either perform a ToHttpResult() on the `ValidatedResult` or add the endpoint filter to the endpoint manually.
Same applies to MapEntityRouteGroup as well.

## FileResponse

Use `ValidatedResult<FileResponse>` as the return value from a `ValidatedHandler` to get an endpoint to stream back a file from an endpoint.

## Mapping Domain to Entity

If you use EF Core and want to have a DTO that is returned the basic scenarios are already handled by defining endpoints return the DTO type 
but then add the interface `IDomainEntity<TEntity>` to your DTO where `TEntity` is the entity type.

These default handlers can be registered by calling:  

    services.AddSliceREndpointDefaultValidatedHandlers();

The trick here is that the endpoint builders need to be registered first so the code can evaluate what default handler interfaces are being referenced.  It then 
attempts to match the desired type to an entity, if not an entity then checks if the type implements the `IEntityMetadata<TEntity>`.  If it all matches up then a default handler can be registered. 
If using `EntityRouteGroupBuilder` then the endpoint itself will already contain EntityMetadata so the DTO does not need the interface itself.

#### Debugging Note:
Code was added to log to the console on startup for any handlers that don't exist.  The Microsoft developer exception page doesn't help with what type is missing 
when it can't be found in the services collection.  So its helpful to log this specific missing handler type at the beginning of the console log.