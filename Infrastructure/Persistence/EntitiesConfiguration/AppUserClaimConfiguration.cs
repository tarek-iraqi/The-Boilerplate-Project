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
    public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.ToTable("UserClaims");

            builder.Property(m => m.Id).HasMaxLength(85);
            builder.Property(m => m.UserId).HasMaxLength(85);
        }
    }
}
