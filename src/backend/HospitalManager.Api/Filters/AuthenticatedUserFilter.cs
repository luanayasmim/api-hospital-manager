using HospitalManager.Communication.Responses;
using HospitalManager.Domain.Repositories.User;
using HospitalManager.Domain.Security.Tokens;
using HospitalManager.Exceptions.ExceptionsBase;
using HospitalManager.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManager.Api.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _tokenValidator;
    private readonly IUserReadOnlyRepository _readOnlyRepository;

    public AuthenticatedUserFilter(IAccessTokenValidator tokenValidator, IUserReadOnlyRepository readOnlyRepository)
    {
        _tokenValidator = tokenValidator;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var userIdentifier = _tokenValidator.ValidateAndGetUserIdentifier(token);
            var exist = await _readOnlyRepository.ExistActiveUserWithIdentifier(userIdentifier);

            if (!exist)
            {
                throw new HospitalManagerException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired") { TokenIsExpired = true });
        }
        catch (HospitalManagerException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));

        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authentication))
        {
            throw new HospitalManagerException(ResourceMessagesException.NO_TOKEN);
        }

        return authentication["Bearer ".Length..].Trim();
    }
}
