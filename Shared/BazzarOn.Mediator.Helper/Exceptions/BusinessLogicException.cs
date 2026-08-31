using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Mediator.Helper.Exceptions;

public class BusinessLogicException : Exception
{
    public Error Error { get; }

    public BusinessLogicException(Error error) : base(error.Description) 
        => Error = error;

    public BusinessLogicException(Error error, Exception innerException) : base(error.Description, innerException)
        => Error = error;
}