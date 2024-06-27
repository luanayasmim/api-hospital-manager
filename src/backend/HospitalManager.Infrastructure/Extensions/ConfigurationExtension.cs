using HospitalManager.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace HospitalManager.Infrastructure.Extensions;
public static class ConfigurationExtension
{
    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    public static DatabaseType DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("DatabaseType");

        return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        //Se tivesse mais uma opção de banco...
        //if(databaseType == Domain.Enums.DatabaseType.SqlServer)
        //    return configuration.GetConnectionString("ConnectionSqlServer");

        return configuration.GetConnectionString("ConnectionSqlServer")!;
    }
}
