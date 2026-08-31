using BazzarOn.Application.User.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BazzarOn.Infrastructure.Persistence.Repositories;

internal static class Configuration
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}