namespace BazzarOn.Application.User.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlMessage,CancellationToken cancellationToken = default);
}