using HospitalManager.Application.Services.Cryptography;
using HospitalManager.Communication.Requests.Login;
using HospitalManager.Communication.Responses.User;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Exceptions.ExceptionsBase;

namespace HospitalManager.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly PasswordEncripter _passwordEncripter;

    public DoLoginUseCase(IUserReadOnlyRepository readOnlyRepository, PasswordEncripter passwordEncripter)
    {
        _readOnlyRepository = readOnlyRepository;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encripteredPassword = _passwordEncripter.Encrypt(request.Password);

        var user = await _readOnlyRepository.GetByEmailAndPassword(request.Email, encripteredPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Id = user.Id,
            Name = user.Name,
        };
    }
}
