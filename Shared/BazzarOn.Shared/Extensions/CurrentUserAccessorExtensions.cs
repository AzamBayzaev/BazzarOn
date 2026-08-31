using System.Security.Claims;
using BazzarOn.Mediator.Helper.Exceptions;

namespace BazzarOn.Shared.Extensions;

public static class CurrentUserAccessorExtensions
{
    public static ClaimsPrincipal GetRequiredUser(this ICurrentUserAccessor accessor)
    {
        ArgumentNullException.ThrowIfNull(accessor);
        return accessor.User ?? throw new UnauthorizedException();
    }

    public static IDisposable BeginScope(this ICurrentUserAccessor accessor, ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(user);

        accessor.User = user;
        return new CurrentUserScope(accessor);
    }

    private sealed class CurrentUserScope : IDisposable
    {
        private readonly ICurrentUserAccessor _accessor;

        public CurrentUserScope(ICurrentUserAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Dispose()
        {
            _accessor.User = null;
        }
    }
}