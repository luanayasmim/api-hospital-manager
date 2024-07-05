using CommonTestUtilities.Entities;
using HospitalManager.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private HospitalManager.Domain.Entities.User _user = default!;
    private string _password = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HospitalManagerDbContext>));

                if(descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<HospitalManagerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                // Adicionando usuário no banco em memória para testes integrados
                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<HospitalManagerDbContext>();

                dbContext.Database.EnsureDeleted();

                StartDatabase(dbContext);
            });
    }
    public Guid GetId() => _user.Id;
    public string GetName() => _user.Name;

    public string GetEmail() => _user.Email;

    public string GetPassword() => _password;
    

    private void StartDatabase(HospitalManagerDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}
