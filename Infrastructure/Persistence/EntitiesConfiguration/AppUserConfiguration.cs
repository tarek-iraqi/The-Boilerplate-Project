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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(name: "Users");

            builder.Property(m => m.Id).HasMaxLength(85);
            builder.Property(m => m.NormalizedEmail).HasMaxLength(85);
            builder.Property(m => m.NormalizedUserName).HasMaxLength(85);
            builder.Property(m => m.PhoneNumber).HasMaxLength(85);
            builder.Property(m => m.Email).HasMaxLength(85);
            builder.Property(m => m.UserName).HasMaxLength(85);

            builder.OwnsOne(p => p.Name, p =>
            {
                p.Property(pp => pp.First).HasMaxLength(85).HasColumnName("FirstName");
                p.Property(pp => pp.Last).HasMaxLength(85).HasColumnName("LastName");
            });


            builder.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired(false);

            builder.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired(false);

            builder.HasMany(e => e.Tokens)
                 .WithOne(e => e.User)
                 .HasForeignKey(ut => ut.UserId)
                 .IsRequired(false);

            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired(false);

        }
    }
}
