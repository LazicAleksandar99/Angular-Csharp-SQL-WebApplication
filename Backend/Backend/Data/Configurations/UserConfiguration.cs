using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Email).IsRequired(true);
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.Firstname).HasMaxLength(30);
            builder.Property(x => x.Firstname).IsRequired(true);

            builder.Property(x => x.Lastname).HasMaxLength(30);
            builder.Property(x => x.Lastname).IsRequired(true);

            builder.Property(x => x.Username).HasMaxLength(30);
            builder.Property(x => x.Username).IsRequired(true);
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(x => x.Password).IsRequired(true);
            builder.Property(x => x.Role).IsRequired(true);
            builder.Property(x => x.Birthday).IsRequired(true);
            builder.Property(x => x.Address).IsRequired(true);
            builder.Property(x => x.Verification).IsRequired(true);
            //builder.Property(x => x.Registrated).IsRequired(true);

            //builder.HasOne(x=> x.CurrentOrder)
            //       .WithOne(x=> x.)

            //builder.HasMany(x => x.UserHistory)
            //       .WithOne(x => x.User)
            //       .HasForeignKey(x => x.UserId);

        }
    }
}
