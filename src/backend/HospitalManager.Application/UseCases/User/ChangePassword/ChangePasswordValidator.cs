using FluentValidation;
using HospitalManager.Application.SharedValidators;
using HospitalManager.Communication.Requests.User;

namespace HospitalManager.Application.UseCases.User.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword)
            .SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
