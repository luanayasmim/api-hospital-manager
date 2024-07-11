using HospitalManager.Communication.Responses.User;

namespace HospitalManager.Application.UseCases.User.Profile;
public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
