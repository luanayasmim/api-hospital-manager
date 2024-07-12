using HospitalManager.Communication.Requests.User;
using HospitalManager.Domain.Repositories;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Domain.Services.LoggedUser;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;

namespace HospitalManager.Application.UseCases.User.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnityOfWork unityOfWork)
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();

        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyRepository.Update(user);

        await _unityOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
