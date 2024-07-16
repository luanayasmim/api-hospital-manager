using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.User.Update;
public class UpdateUserInvalidTokenTest : HospitalManagerClassFixture
{
    private const string _method = "user";
    private readonly Guid _userIdentifier;

    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Error_TokenInvalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(_method, request, token: "invalidToken");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_WithoutToken()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(_method, request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_TokenWithUserNotFound()
    { 
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(_method, request, token: "invalidToken");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
