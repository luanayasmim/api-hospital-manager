﻿namespace HospitalManager.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
    public Task<bool> ExistActiveUserWithIdentifier(Guid identifier);
}
