using FluentValidation;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Exceptions;

namespace HospitalManager.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesExceptions.NAME_EMPTY);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesExceptions.EMAIL_EMPTY);

        RuleFor(user => user.Password.Length)
            .GreaterThan(6)
            .WithMessage(ResourceMessagesExceptions.PASSWORD_INVALID);

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage(ResourceMessagesExceptions.EMAIL_INVALID);
        });
    }
}
