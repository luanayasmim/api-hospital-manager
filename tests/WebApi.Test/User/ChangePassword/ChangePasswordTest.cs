using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using HospitalManager.Communication.Requests.Login;
using HospitalManager.Communication.Requests.User;
using HospitalManager.Exceptions;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using System.Globalization;

namespace WebApi.Test.User.ChangePassword;
public class ChangePasswordTest : HospitalManagerClassFixture
{
    private const string _method = "user/change-password";
    private readonly Guid _userIdentifier;
    private readonly string _email;
    private readonly string _password;


    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _email = factory.GetEmail();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(_method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        response = await DoPost("login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost("login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NewPasswordEmpty(string culture)
    {
        var request = new RequestChangePasswordJson
        {
            Password = _password,
            NewPassword = string.Empty
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(_method, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(c=>c.GetString()!.Equals(expectedMessage));
    }
}
