using HospitalManager.Domain.Security.Cryptography;
using HospitalManager.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Crypthography;
public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Sha512Encripter("test");
}
