using System.Reflection;
using BazzarOn.Application.User;
using Microsoft.Extensions.DependencyInjection;

namespace BazzarOn.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddSingleton<UserMapper>();

        return services;
    }
}