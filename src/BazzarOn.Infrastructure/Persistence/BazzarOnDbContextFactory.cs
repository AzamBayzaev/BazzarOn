using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BazzarOn.Infrastructure.Persistence;

public class BazzarOnDbContextFactory : IDesignTimeDbContextFactory<BazzarDbContext>
{
    public BazzarDbContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var apiPath = Directory.Exists(Path.Combine(currentDirectory, "../BazzarOn.Api"))
            ? Path.Combine(currentDirectory, "../BazzarOn.Api")
            : Path.Combine(currentDirectory, "BazzarOn  .Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена в appsettings.json.");
        }

        var builder = new DbContextOptionsBuilder<BazzarDbContext>();

        builder.UseNpgsql(connectionString, options => 
            options.MigrationsAssembly(typeof(BazzarDbContext).Assembly.FullName));

        return new BazzarDbContext(builder.Options);
    }
}