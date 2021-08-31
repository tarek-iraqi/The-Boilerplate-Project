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
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
    {
        public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            builder.ToTable("UserTokens");

            builder.Property(m => m.UserId).HasMaxLength(85);
            builder.Property(m => m.LoginProvider).HasMaxLength(85);
            builder.Property(m => m.Name).HasMaxLength(85);
        }
    }
}
