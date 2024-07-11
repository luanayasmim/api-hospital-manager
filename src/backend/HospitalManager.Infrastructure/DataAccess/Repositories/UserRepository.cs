﻿using HospitalManager.Domain.Entities;
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

    public async Task<bool> ExistActiveUserWithIdentifier(Guid identifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(identifier) && user.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password) && user.Active);
    }
}
