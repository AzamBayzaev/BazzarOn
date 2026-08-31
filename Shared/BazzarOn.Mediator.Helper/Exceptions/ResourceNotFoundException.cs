using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Mediator.Helper.Exceptions;

public class ResourceNotFoundException : BusinessLogicException
{
    public ResourceNotFoundException(Error error) : base(error) { }

    public ResourceNotFoundException(Error error, Exception innerException) : base(error, innerException) { }
}
