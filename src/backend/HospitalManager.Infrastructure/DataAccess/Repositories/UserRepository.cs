using HospitalManager.Domain.Entities;
using HospitalManager.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace HospitalManager.Infrastructure.DataAccess.Repositories;
public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly HospitalManagerDbContext _dbContext;

    public UserRepository(HospitalManagerDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user)
        => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
        => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
}
