using BazzarOn.Domain.Entities;
using BazzarOn.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BazzarOn.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .HasConversion(x => x.Value, x => new Username(x))
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.Email)
            .HasConversion(x => x.Value, x => new Domain.ValueObjects.Email(x))
            .HasMaxLength(40)
            .IsRequired();
        
        builder.Property(x => x.Password)
            .HasConversion(x => x.Value, x => new Password(x))
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.UserRole)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}