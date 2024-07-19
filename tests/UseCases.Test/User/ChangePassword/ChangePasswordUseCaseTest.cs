using CommonTestUtilities.Crypthography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using HospitalManager.Application.UseCases.User.ChangePassword;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;
using HospitalManager.Communication.Requests.User;

namespace UseCases.Test.User.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.Should().Be(passwordEncripter.Encrypt(request.NewPassword));
    }

    [Fact]
    public async Task Error_NewPasswordEmpty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            Password = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(error => error.ErrorMessages.Count == 1 && error.ErrorMessages.Contains(ResourceMessagesException.PASSWORD_EMPTY));

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.Should().Be(passwordEncripter.Encrypt(password));
    }

    [Fact]
    public async Task Error_CurrentPasswordIsDifferent()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(error=> error.ErrorMessages.Count == 1 && error.ErrorMessages.Contains(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.Should().Be(passwordEncripter.Encrypt(password));
    }

    private static ChangePasswordUseCase CreateUseCase(HospitalManager.Domain.Entities.User user)
    {
        var unityOfWork = UnityOfWorkBuilder.Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, updateOnlyRepository, unityOfWork, passwordEncripter);
    }
}
