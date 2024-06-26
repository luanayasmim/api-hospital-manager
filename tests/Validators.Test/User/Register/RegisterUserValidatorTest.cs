using CommonTestUtilities.Requests.User;
using FluentAssertions;
using HospitalManager.Application.UseCases.User.Register;
using HospitalManager.Exceptions;

namespace Validators.Test.User.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Sucess()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_NameIsEmpty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.NAME_EMPTY));
    }

    [Fact]
    public void Error_EmailIsEmpty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_EmailIsInvalid()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "fdghdjiksdalfkjasl";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_INVALID));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_PasswordInvalid(int passwordLength)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.PASSWORD_INVALID));
    }
}

