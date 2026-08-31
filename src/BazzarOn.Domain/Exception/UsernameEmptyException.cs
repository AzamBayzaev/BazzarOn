namespace BazzarOn.Domain.Exception;

public class UsernameEmptyException : DomainException
{
    public UsernameEmptyException() 
        : base("Username cannot be null or empty.") { }
}