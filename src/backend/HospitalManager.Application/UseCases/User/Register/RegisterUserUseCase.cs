using AutoMapper;
using HospitalManager.Application.Services.Cryptography;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Communication.Responses.User;
using HospitalManager.Domain.Repositories;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;

namespace HospitalManager.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnityOfWork unityOfWork,
        IMapper mapper,
        PasswordEncripter passwordEncripter)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);

        await _unityOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Id = user.Id,
            Name = user.Name,
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExist)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
