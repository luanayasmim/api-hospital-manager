namespace HospitalManager.Domain.Repositories.User;
public interface IUserUpdateOnlyRepository
{
    public Task<Entities.User> GetById(Guid id);
    public void Update(Entities.User user);
}
