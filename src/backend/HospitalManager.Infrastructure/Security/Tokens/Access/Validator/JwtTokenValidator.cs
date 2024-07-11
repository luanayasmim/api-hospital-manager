using HospitalManager.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HospitalManager.Infrastructure.Security.Tokens.Access.Validator;
internal class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey;

    public JwtTokenValidator(string signingKey) => _signingKey = signingKey;

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameter = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principalClaims = tokenHandler.ValidateToken(token, validationParameter, out _);

        var userIdentifier = principalClaims.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userIdentifier);
    }
}
