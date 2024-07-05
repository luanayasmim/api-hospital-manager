using CommonTestUtilities.Requests.Login;
using FluentAssertions;
using HospitalManager.Application.UseCases.Login.DoLogin;
using HospitalManager.Communication.Requests.Login;
using HospitalManager.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : HospitalManagerClassFixture
{
    private readonly string _method = "login";

    private readonly Guid _id;
    private readonly string _name;
    private readonly string _email;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _id = factory.GetId();
        _name = factory.GetName();
        _email = factory.GetEmail();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Sucess()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(_method, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetGuid().Should().NotBeEmpty().And.Be(_id);
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrEmpty().And.Be(_name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_LoginInvalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(_method, request, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
