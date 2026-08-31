using BazzarOn.Infrastructure.Persistence.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BazzarOn.Infrastructure.Persistence;

public class DatabaseInitializer
{
    private readonly BazzarDbContext _context;
    private readonly IEnumerable<IDatabaseSeeder> _seeders;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        BazzarDbContext context,
        IEnumerable<IDatabaseSeeder> seeders,
        ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _seeders = seeders; 
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_context.Database.IsRelational())
            {
                await _context.Database.MigrateAsync(cancellationToken);
            }
            
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
            throw;
        }
    }
}