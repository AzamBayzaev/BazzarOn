namespace BazzarOn.Infrastructure.Persistence.Seeders.Interfaces;

public interface IDatabaseSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}