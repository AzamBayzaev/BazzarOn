namespace BazzarOn.Domain.Exception;

public class InvalidUsernameFormatException : DomainException
{
    public InvalidUsernameFormatException() 
        : base("Username can only contain letters, digits, underscores (_), and dots (.).") { }
}