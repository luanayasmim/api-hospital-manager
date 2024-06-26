using HospitalManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManager.Infrastructure.DataAccess;
public class HospitalManagerDbContext : DbContext
{
    public HospitalManagerDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HospitalManagerDbContext).Assembly);
    }
}
