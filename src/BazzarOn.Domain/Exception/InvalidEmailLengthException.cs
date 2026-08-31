namespace BazzarOn.Domain.Exception;

public class InvalidEmailLengthException : DomainException
{
    public InvalidEmailLengthException()
        : base("Email length must be between 5 and 50 characters.") { }
}