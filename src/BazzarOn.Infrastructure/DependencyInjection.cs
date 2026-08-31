using System.Reflection;
using BazzarOn.Application.User.Interfaces;
using BazzarOn.Infrastructure.BackgroundServices;
using BazzarOn.Infrastructure.Email;
using BazzarOn.Infrastructure.Identity;
using BazzarOn.Infrastructure.Persistence;
using BazzarOn.Infrastructure.Persistence.Repositories;
using BazzarOn.Infrastructure.Persistence.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BazzarOn.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена в конфигурации.");

        services.AddDbContext<BazzarDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(BazzarDbContext).Assembly.FullName)));

        services.AddIdentity<AppUserIdentity, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<BazzarDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();

        services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();

        services.AddRepositories(); 
        services.AddScoped<DatabaseInitializer>();
        services.AddHostedService<SoftDeleteCleanupService>();
        services.AddDatabaseSeeders();

        return services;
    }
    
    private static IServiceCollection AddDatabaseSeeders(this IServiceCollection services)
    {
        var seederTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IDatabaseSeeder).IsAssignableFrom(t) 
                        && !t.IsInterface 
                        && !t.IsAbstract);

        foreach (var seederType in seederTypes)
        {
            services.AddScoped(typeof(IDatabaseSeeder), seederType);
        }

        return services;
    }
}