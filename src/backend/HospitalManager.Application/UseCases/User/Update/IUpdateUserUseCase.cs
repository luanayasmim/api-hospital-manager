using HospitalManager.Communication.Requests.User;

namespace HospitalManager.Application.UseCases.User.Update;
public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}
