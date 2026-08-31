namespace BazzarOn.Application.User.Interfaces;

public interface IEmailVerificationService
{
    Task SendConfirmationEmailAsync(Guid userId,CancellationToken cancellationToken);
    Task<bool> ConfirmEmailAsync(Guid userId, string token,CancellationToken cancellationToken);
    Task ResendConfirmationEmailAsync(Guid userId,CancellationToken cancellationToken);
}