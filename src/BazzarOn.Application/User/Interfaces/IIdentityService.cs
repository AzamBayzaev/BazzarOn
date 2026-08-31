namespace BazzarOn.Application.User.Interfaces;

public interface IIdentityService
{
    Task CreateUserAsync(
        Guid userId, 
        string username, 
        string email, 
        string password, 
        CancellationToken cancellationToken = default);
}