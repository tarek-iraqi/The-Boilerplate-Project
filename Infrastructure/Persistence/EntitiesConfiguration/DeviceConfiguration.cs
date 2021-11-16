using Domain.Entities;
using Helpers.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.Property(p => p.Model).HasMaxLength(500);
            builder.Property(p => p.Token).HasMaxLength(2000);
            builder.Property(p => p.UserId).HasConversion<string>().HasMaxLength(85);
            builder.Property(p => p.DeviceLanguage).HasMaxLength(10).HasDefaultValue(KeyValueConstants.EnglishLanguage);

            builder.HasOne(p => p.User)
                .WithMany(p => p.Devices)
                .HasForeignKey(p => p.UserId);
        }
    }
}
