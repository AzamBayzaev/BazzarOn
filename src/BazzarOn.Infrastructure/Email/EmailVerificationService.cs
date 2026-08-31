using Microsoft.AspNetCore.Identity;
using BazzarOn.Application.User.Interfaces;
using BazzarOn.Application.User.Repositories;
using BazzarOn.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace BazzarOn.Infrastructure.Email;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly UserManager<AppUserIdentity> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    private static readonly string TokenProvider = TokenOptions.DefaultEmailProvider;
    private const string TokenPurpose = "EmailConfirmation";

    public EmailVerificationService(
        UserManager<AppUserIdentity> userManager,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task SendConfirmationEmailAsync(Guid userId,CancellationToken cancellationToken)
    {
        var identityUser = await GetIdentityUserAsync(userId);
        var domainUser = await GetDomainUserAsync(userId);

        var code = await _userManager.GenerateUserTokenAsync(identityUser, TokenProvider, TokenPurpose);

        var html = $@"
            <h2>Confirm email</h2>
            <p>Your code Confirmation:</p>
            <h1 style='font-size: 24px; letter-spacing: 4px;'>{code}</h1>";

        await _emailService.SendEmailAsync(domainUser.Email.Value, "Code for Confirmation", html);
    }

    public async Task<bool> ConfirmEmailAsync(Guid userId, string code,CancellationToken cancellationToken)
    {
        var identityUser = await GetIdentityUserAsync(userId);

        var isValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenProvider, TokenPurpose, code);

        if (!isValid)
            return false;

        var domainUser = await GetDomainUserAsync(userId);
        domainUser.VerifyEmail();
        await _userRepository.SaveChangesAsync();

        return true;
    }

    public Task ResendConfirmationEmailAsync(Guid userId,CancellationToken cancellationToken) => SendConfirmationEmailAsync(userId,cancellationToken);

    private async Task<AppUserIdentity> GetIdentityUserAsync(Guid userId)
    {
        var identityUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        return identityUser ?? throw new InvalidOperationException("Identity user not found");
    }

    private async Task<Domain.Entities.User> GetDomainUserAsync(Guid userId)
    {
        var domainUser = await _userRepository.GetByIdAsync(userId);
        return domainUser ?? throw new InvalidOperationException("Domain user not found");
    }
}