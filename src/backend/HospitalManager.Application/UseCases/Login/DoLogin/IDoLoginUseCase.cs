using HospitalManager.Communication.Requests.Login;
using HospitalManager.Communication.Responses.User;

namespace HospitalManager.Application.UseCases.Login.DoLogin;
public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
