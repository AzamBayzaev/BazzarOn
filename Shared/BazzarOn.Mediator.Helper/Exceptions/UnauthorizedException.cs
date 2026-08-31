using BazzarOn.Mediator.Helper.Common;
using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Mediator.Helper.Exceptions;

public class UnauthorizedException : BusinessLogicException
{
    public UnauthorizedException() : this(GeneralErrors.Unauthorized) { }

    public UnauthorizedException(Error error) : base(error) { }

    public UnauthorizedException(Error error, Exception innerException) : base(error, innerException) { }
}
