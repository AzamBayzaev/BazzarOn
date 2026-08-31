using BazzarOn.Domain.Entities;
using BazzarOn.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BazzarOn.Infrastructure.Persistence;

public class BazzarDbContext : IdentityDbContext<AppUserIdentity, IdentityRole<Guid>, Guid>
{
    public DbSet<User> DomainUsers => Set<User>();

    public BazzarDbContext(DbContextOptions option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(BazzarDbContext).Assembly);

        builder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplySoftDelete();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplySoftDelete();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplySoftDelete()
    {
        var entries = ChangeTracker
            .Entries<User>()
            .Where(x => x.State == EntityState.Deleted || (x.State == EntityState.Modified && x.Entity.IsDeleted))
            .ToList();

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt ??= DateTime.UtcNow;
        }
    }
}