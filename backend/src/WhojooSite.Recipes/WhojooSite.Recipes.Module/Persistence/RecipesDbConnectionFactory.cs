using System.Data;

using Microsoft.Extensions.Configuration;

using Npgsql;

namespace WhojooSite.Recipes.Module.Persistence;

internal sealed class RecipesDbConnectionFactory(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString(DataSchemaConstants.DbName));
    }
}