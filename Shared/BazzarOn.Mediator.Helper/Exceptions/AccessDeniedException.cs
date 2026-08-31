using BazzarOn.Mediator.Helper.Common;
using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Mediator.Helper.Exceptions;

public sealed class AccessDeniedException : BusinessLogicException
{
    public AccessDeniedException() : base(GeneralErrors.AccessDenied) {}
    
    public AccessDeniedException(Error error) : base(error) {}

    public AccessDeniedException(Error error, Exception innerException) : base(error, innerException) {}
}