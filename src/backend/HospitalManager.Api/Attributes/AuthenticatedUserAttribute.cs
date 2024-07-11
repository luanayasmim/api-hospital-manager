using HospitalManager.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManager.Api.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
