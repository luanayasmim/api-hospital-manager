﻿using CommonTestUtilities.Crypthography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using HospitalManager.Application.UseCases.User.Register;
using HospitalManager.Exceptions;
using HospitalManager.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_EmailAlreadyResgistered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(error => error.ErrorMessages.Count == 1 && error.ErrorMessages.Contains(ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED));
    }

    [Fact]
    public async Task Error_NameIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(error => error.ErrorMessages.Count == 1 && error.ErrorMessages.Contains(ResourceMessagesExceptions.NAME_EMPTY));
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var mapper = AutoMappingBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if(!string.IsNullOrEmpty(email))
            readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(readOnlyRepositoryBuilder.Build(), writeOnlyRepository, unityOfWork, mapper, passwordEncripter);
    }
}