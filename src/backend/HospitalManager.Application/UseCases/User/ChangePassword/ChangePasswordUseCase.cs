using FluentValidation.Results;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Domain.Repositories;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Domain.Security.Cryptography;
using HospitalManager.Domain.Services.LoggedUser;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;

namespace HospitalManager.Application.UseCases.User.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUnityOfWork unityOfWork,
        IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _unityOfWork = unityOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _updateOnlyRepository.Update(user);

        await _unityOfWork.Commit();

    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncripted = _passwordEncripter.Encrypt(request.Password);

        if (!currentPasswordEncripted.Equals(loggedUser.Password))
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesExceptions.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(error=>error.ErrorMessage).ToList());
    }
}
