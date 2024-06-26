using HospitalManager.Domain.Repositories;

namespace HospitalManager.Infrastructure.DataAccess;
public class UnityOfWork : IUnityOfWork
{
    private readonly HospitalManagerDbContext _dbContext;

    public UnityOfWork(HospitalManagerDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
