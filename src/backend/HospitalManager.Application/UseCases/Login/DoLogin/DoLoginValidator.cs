using FluentValidation;
using HospitalManager.Communication.Requests.Login;
using HospitalManager.Exceptions;

namespace HospitalManager.Application.UseCases.Login.DoLogin;
public class DoLoginValidator : AbstractValidator<RequestLoginJson>
{
    public DoLoginValidator()
    {
        RuleFor(login=>login.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        When(login => !string.IsNullOrEmpty(login.Email), () =>
        {
            RuleFor(login => login.Email)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}
