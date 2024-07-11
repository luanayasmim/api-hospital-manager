using HospitalManager.Domain.Entities;

namespace HospitalManager.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    public Task<User> User();
}
