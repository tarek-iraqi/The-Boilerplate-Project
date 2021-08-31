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
    public class AppUserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            builder.ToTable("UserLogins");

            builder.Property(m => m.LoginProvider).HasMaxLength(85);
            builder.Property(m => m.ProviderKey).HasMaxLength(85);
            builder.Property(m => m.UserId).HasMaxLength(85);
            builder.Property(m => m.UserId).HasMaxLength(85);
        }
    }
}
