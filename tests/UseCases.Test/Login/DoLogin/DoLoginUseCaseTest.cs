using CommonTestUtilities.Crypthography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests.Login;
using FluentAssertions;
using HospitalManager.Application.UseCases.Login.DoLogin;
using HospitalManager.Communication.Requests.Login;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login.DoLogin;
public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password,
        });

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
    }

    [Fact]
    public async Task Error_InvalidUser()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<InvalidLoginException>().Where(error => error.Message.Equals(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID));
    }

    private static DoLoginUseCase CreateUseCase(HospitalManager.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);

        return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter);
    }
}
