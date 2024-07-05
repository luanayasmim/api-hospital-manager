using FluentMigrator.Runner;
using HospitalManager.Domain.Enums;
using HospitalManager.Domain.Repositories;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Domain.Security.Tokens;
using HospitalManager.Infrastructure.DataAccess;
using HospitalManager.Infrastructure.DataAccess.Repositories;
using HospitalManager.Infrastructure.Extensions;
using HospitalManager.Infrastructure.Security.Tokens.Access.Generator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HospitalManager.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddTokens(services, configuration);

        if(configuration.IsUnitTestEnvironment())
            return;

        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.SqlServer)
        {
            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }

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

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(options => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }
}
