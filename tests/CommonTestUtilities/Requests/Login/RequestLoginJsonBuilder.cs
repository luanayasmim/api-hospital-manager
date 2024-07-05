using Bogus;
using HospitalManager.Communication.Requests.Login;

namespace CommonTestUtilities.Requests.Login;
public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(user => user.Email, (f) => f.Internet.Email())
            .RuleFor(user => user.Password, (f) => f.Internet.Password());
    }
}
