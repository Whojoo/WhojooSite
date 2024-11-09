using System.Data;

using Microsoft.Extensions.Configuration;

using WhojooSite.Common.Persistence;

namespace WhojooSite.Recipes.Module.Persistence;

internal sealed class RecipesDbConnectionFactory(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly IConfiguration _configuration = configuration;

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString(DataSchemaConstants.ConnectionStringName);
        return _dbConnectionFactory.CreateConnection(connectionString!);
    }
}