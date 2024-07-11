using HospitalManager.Api.Attributes;
using HospitalManager.Application.UseCases.User.Register;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Communication.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManager.Api.Controllers;

[AuthenticatedUser]
public class UserController : HospitalManagerBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
}
