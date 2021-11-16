using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
    {
        public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            builder.ToTable("UserTokens");

            builder.Property(m => m.UserId).HasConversion<string>().HasMaxLength(85);
            builder.Property(m => m.LoginProvider).HasMaxLength(85);
            builder.Property(m => m.Name).HasMaxLength(85);
        }
    }
}
