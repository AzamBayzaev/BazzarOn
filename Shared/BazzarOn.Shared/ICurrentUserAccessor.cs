using System.Security.Claims;

namespace BazzarOn.Shared;

public interface ICurrentUserAccessor
{
    ClaimsPrincipal? User { get; set; }
}

public sealed class CurrentUserAccessor : ICurrentUserAccessor
{
    private static readonly AsyncLocal<CurrentUserHolder> _current = new();

    public ClaimsPrincipal? User
    {
        get => _current.Value?.User;
        set
        {
            var holder = _current.Value;

            if (holder != null)
            {
                holder.User = null;
            }

            if (value is not null)
            {
                _current.Value = new CurrentUserHolder
                {
                    User = value,
                };
            }
        }
    }

    private sealed class CurrentUserHolder
    {
        public ClaimsPrincipal? User;
    }
}