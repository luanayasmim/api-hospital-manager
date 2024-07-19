using FluentValidation;
using HospitalManager.Application.SharedValidators;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Exceptions;

namespace HospitalManager.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator<RequestRegisterUserJson>());

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}
