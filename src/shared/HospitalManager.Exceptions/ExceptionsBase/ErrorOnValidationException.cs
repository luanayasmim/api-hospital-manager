namespace HospitalManager.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : HospitalManagerException
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> errorMessages) => ErrorMessages = errorMessages;
}
