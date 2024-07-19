namespace HospitalManager.Exceptions.ExceptionsBase;
public class InvalidLoginException : HospitalManagerException
{
    public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID) { }
}
