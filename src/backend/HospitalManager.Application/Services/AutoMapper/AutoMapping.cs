using AutoMapper;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Communication.Responses.User;

namespace HospitalManager.Application.Services.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, option => option.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
    }
}
