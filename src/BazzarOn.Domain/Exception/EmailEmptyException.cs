namespace BazzarOn.Domain.Exception;

public class EmailEmptyException : DomainException
{
    public EmailEmptyException() 
        : base("Email cannot be null or empty.") { }
}