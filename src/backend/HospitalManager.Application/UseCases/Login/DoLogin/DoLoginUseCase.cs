using HospitalManager.Communication.Requests.Login;
using HospitalManager.Communication.Responses;
using HospitalManager.Communication.Responses.User;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Domain.Security.Cryptography;
using HospitalManager.Domain.Security.Tokens;
using HospitalManager.Exceptions.ExceptionsBase;

namespace HospitalManager.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _readOnlyRepository = readOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        Validate(request);

        var encripteredPassword = _passwordEncripter.Encrypt(request.Password);

        var user = await _readOnlyRepository.GetByEmailAndPassword(request.Email, encripteredPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
            }
        };
    }

    private void Validate(RequestLoginJson request)
    {
        var validator = new DoLoginValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

    }
}
