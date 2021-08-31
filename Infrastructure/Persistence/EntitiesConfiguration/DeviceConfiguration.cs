using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntitiesConfiguration
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.Property(p => p.Model).HasMaxLength(500);
            builder.Property(p => p.Token).HasMaxLength(2000);

            builder.HasOne(p => p.User)
                .WithMany(p => p.Devices)
                .HasForeignKey(p => p.UserId);
        }
    }
}
