using Microsoft.AspNetCore.Identity;

namespace BazzarOn.Infrastructure.Identity;

public class AppUserIdentity : IdentityUser<Guid>
{
    public Guid UserId { get; set; }
}