namespace BazzarOn.Domain.Exception;

public class InvalidUsernameLengthException : DomainException
{
    public InvalidUsernameLengthException()
        : base("Username length must be between 3 and 20 characters.") { }
}