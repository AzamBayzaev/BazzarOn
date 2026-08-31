namespace BazzarOn.Domain.Exception;

public class PasswordEmptyException : DomainException
{
    public PasswordEmptyException() 
        : base("Password cannot be null or empty.") { }
}