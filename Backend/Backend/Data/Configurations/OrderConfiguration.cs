using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Price).IsRequired(true);

            builder.Property(x => x.DeliveryTime).IsRequired(true);

            builder.Property(x => x.DeliveryDate).IsRequired(true);

            builder.Property(x => x.SuccessfulDelivery).IsRequired(true);

            builder.Property(x => x.OrderAccepted).IsRequired(true);

            builder.Property(x => x.CurrentOrdeer).IsRequired(true);


            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(x => x.UserId);
                   
            //builder.Property(x => x.UserId).IsRequired(true);
            //builder.Property(x => x.Role).IsRequired(true);
            //builder.Property(x => x.Birthday).IsRequired(true);
            //builder.Property(x => x.Adress).IsRequired(true);


            //builder.HasMany(x => x.UserHistory)
            //       .WithOne(x => x.User)
            //       .HasForeignKey(x => x.UserId);

        }
    }
}
