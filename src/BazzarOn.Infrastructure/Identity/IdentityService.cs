using BazzarOn.Application.User.Interfaces;
using BazzarOn.Mediator.Helper.Common.Models;
using BazzarOn.Mediator.Helper.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BazzarOn.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUserIdentity> _userManager;

    public IdentityService(UserManager<AppUserIdentity> userManager)
    {
        _userManager = userManager;
    }

    public async Task CreateUserAsync(
        Guid userId, 
        string username, 
        string email, 
        string password, 
        CancellationToken cancellationToken = default)
    {
        var identityUser = new AppUserIdentity 
        { 
            Id = userId, 
            UserName = username, 
            Email = email 
        };

        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description).ToArray());
            var error = new Error("Identity.CreationFailed", errorMessages);
            throw new BusinessLogicException(error);
        }
    }
}