using CommonTestUtilities.Tokens;
using FluentAssertions;
using HospitalManager.Communication.Requests.User;
using System.Net;

namespace WebApi.Test.User.ChangePassword;
public class ChangePasswordInvalidTokenTest : HospitalManagerClassFixture
{
    private const string _method = "user/change-password";

    public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Error_TokenInvalid()
    {
        var request = new RequestChangePasswordJson();

        var response = await DoPut(_method, request, token: "invalidToken");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_WithoutToken()
    {
        var request = new RequestChangePasswordJson();

        var response = await DoPut(_method, request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_TokenWithUserNotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var request = new RequestChangePasswordJson();

        var response = await DoPut(_method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
