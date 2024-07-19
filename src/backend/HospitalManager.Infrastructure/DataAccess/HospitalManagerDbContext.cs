using HospitalManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManager.Infrastructure.DataAccess;
public class HospitalManagerDbContext : DbContext
{
    public HospitalManagerDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<HealthInsureance> HealthInsureances { get; set; }
    public DbSet<MedicalReport> MedicalReports { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HospitalManagerDbContext).Assembly);
    }
}
