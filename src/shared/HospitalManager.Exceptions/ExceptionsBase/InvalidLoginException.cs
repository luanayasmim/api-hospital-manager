namespace HospitalManager.Exceptions.ExceptionsBase;
public class InvalidLoginException : HospitalManagerException
{
    public InvalidLoginException() : base(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID) { }
}
