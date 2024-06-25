using AutoMapper;
using HospitalManager.Communication.Requests.User;

namespace HospitalManager.Application.Services.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, option => option.Ignore());
    }
}
