using HospitalManager.Application.UseCases.Login.DoLogin;
using HospitalManager.Communication.Requests.Login;
using HospitalManager.Communication.Responses;
using HospitalManager.Communication.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManager.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class LoginController : HospitalManagerBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
