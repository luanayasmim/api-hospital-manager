using HospitalManager.Application.Services.Cryptography;

namespace CommonTestUtilities.Crypthography;
public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new PasswordEncripter("test");
}
