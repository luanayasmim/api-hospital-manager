using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using HospitalManager.Application.UseCases.User.Update;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update;
public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_NameEmpty()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(error => error.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_EmailAlreadyRegistered()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(error => error.ErrorMessages.Count == 1 && error.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(HospitalManager.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var unityOfWork = UnityOfWorkBuilder.Build();

        if(!string.IsNullOrEmpty(email))
            readOnlyRepository.ExistActiveUserWithEmail(email!);

        return new UpdateUserUseCase(loggedUser, updateOnlyRepository, readOnlyRepository.Build(), unityOfWork);

    }
}
