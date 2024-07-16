using HospitalManager.Domain.Entities;
using HospitalManager.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _updateOnlyRepository;

    public UserUpdateOnlyRepositoryBuilder() => _updateOnlyRepository = new Mock<IUserUpdateOnlyRepository>();

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _updateOnlyRepository.Setup(x=>x.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public IUserUpdateOnlyRepository Build() => _updateOnlyRepository.Object;
}
