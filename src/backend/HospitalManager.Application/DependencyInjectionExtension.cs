using AutoMapper;
using HospitalManager.Application.Services.AutoMapper;
using HospitalManager.Application.Services.Cryptography;
using HospitalManager.Application.UseCases.User.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManager.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddPasswordEncripter(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        })
        .CreateMapper());
    }

    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        services.AddScoped(option => new PasswordEncripter(additionalKey!));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }
}
