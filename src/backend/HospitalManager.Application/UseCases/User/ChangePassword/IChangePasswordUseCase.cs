using HospitalManager.Communication.Requests.User;

namespace HospitalManager.Application.UseCases.User.ChangePassword;
public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
