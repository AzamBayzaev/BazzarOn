namespace BazzarOn.Domain.Exception;

public class InvalidPasswordLengthException : DomainException
{
    public InvalidPasswordLengthException() 
        : base("Password length must be between 7 and 25 characters.") { }
}