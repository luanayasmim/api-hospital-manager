using FluentMigrator.Runner;
using HospitalManager.Domain.Enums;
using HospitalManager.Domain.Repositories;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Infrastructure.DataAccess;
using HospitalManager.Infrastructure.DataAccess.Repositories;
using HospitalManager.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HospitalManager.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.SqlServer)
        {
            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }

        AddRepositories(services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<HospitalManagerDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        //User dependency injection
        services.AddScoped<IUnityOfWork, UnityOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("HospitalManager.Infrastructure")).For.All();
        });
    }
}
