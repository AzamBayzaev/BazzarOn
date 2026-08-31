using BazzarOn.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BazzarOn.Infrastructure.BackgroundServices;

public class SoftDeleteCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SoftDeleteCleanupService> _logger;

    public SoftDeleteCleanupService(IServiceProvider serviceProvider, ILogger<SoftDeleteCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOldDeletedUsersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при фоновой очистке устаревших пользователей");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task CleanupOldDeletedUsersAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BazzarDbContext>();

        var thresholdDate = DateTime.UtcNow.AddDays(-30);

        int deletedCount = await dbContext.DomainUsers
            .IgnoreQueryFilters()
            .Where(u => u.IsDeleted && u.DeletedAt <= thresholdDate)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedCount > 0)
            _logger.LogInformation("Фоновая служба удалила {Count} пользователей из базы (старше 30 дней).", deletedCount);
    }
}