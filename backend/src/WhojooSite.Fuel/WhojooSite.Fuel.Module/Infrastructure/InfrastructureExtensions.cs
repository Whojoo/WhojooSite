using Dapper;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.FuelEntries;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;
using WhojooSite.Fuel.Module.Infrastructure.Persistence;
using WhojooSite.Fuel.Module.Infrastructure.Persistence.FuelEntries;
using WhojooSite.Fuel.Module.Infrastructure.Persistence.ObjectTypes;
using WhojooSite.Fuel.Module.Infrastructure.Persistence.TrackableObjects;

namespace WhojooSite.Fuel.Module.Infrastructure;

internal static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new FuelEntryId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ObjectTypeId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new TrackableObjectId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new OwnerId.DapperTypeHandler());

        return services
            .AddSingleton<IFuelConnectionFactory>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                return new NpgsqlDapperFuelConnectionFactory(config.GetConnectionString("ServerDb")!);
            })
            .AddSingleton<IFuelEntryRepository, NpgsqlDapperFuelEntriesRepository>()
            .AddSingleton<ITrackableObjectRepository, NpgsqlDapperTrackableObjectsRepository>()
            .AddSingleton<NpgsqlDapperObjectTypesRepository>()
            .AddSingleton<IObjectTypeRepository>(provider =>
            {
                var repository = provider.GetRequiredService<NpgsqlDapperObjectTypesRepository>();
                var memoryCache = provider.GetRequiredService<IMemoryCache>();
                return new MemoryCacheObjectTypesRepository(memoryCache, repository);
            });
    }
}