using AutoMapper;
using HospitalManager.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;
public class AutoMappingBuilder
{
    public static IMapper Build()
    {
        return new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper();
    }
}
