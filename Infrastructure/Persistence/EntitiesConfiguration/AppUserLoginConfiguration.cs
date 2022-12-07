using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration;

public class AppUserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
{
    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {
        builder.ToTable("UserLogins");

        builder.Property(m => m.LoginProvider).HasMaxLength(85);
        builder.Property(m => m.ProviderKey).HasMaxLength(85);
        builder.Property(m => m.UserId).HasConversion<string>().HasMaxLength(85);
    }
}