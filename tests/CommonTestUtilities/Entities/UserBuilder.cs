using Bogus;
using CommonTestUtilities.Crypthography;
using HospitalManager.Domain.Entities;

namespace CommonTestUtilities.Entities;
public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var password = new Faker().Internet.Password();

        var id = Guid.Parse("b7c50b46-1993-217a-4164-0edbaef6ab14");

        var user = new Faker<User>()
            .RuleFor(user => user.Id, () => id)
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.UserIdentifier, f => f.Random.Guid())
            .RuleFor(user => user.Password, (f) => passwordEncripter.Encrypt(password));

        return (user, password);
    }
}
