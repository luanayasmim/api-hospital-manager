using HospitalManager.Domain.Entities;
using HospitalManager.Domain.Security.Tokens;
using HospitalManager.Domain.Services.LoggedUser;
using HospitalManager.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HospitalManager.Infrastructure.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly HospitalManagerDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(HospitalManagerDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}
