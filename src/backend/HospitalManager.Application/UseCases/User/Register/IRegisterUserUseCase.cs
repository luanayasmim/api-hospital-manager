using HospitalManager.Communication.Requests.User;
using HospitalManager.Communication.Responses.User;

namespace HospitalManager.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
