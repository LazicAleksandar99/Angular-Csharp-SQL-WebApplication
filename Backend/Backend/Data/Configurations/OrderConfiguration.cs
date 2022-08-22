using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(x => x.UserId);

            builder.Property(x => x.Comment).IsRequired(true);

            builder.Property(x => x.PaymentStatus).IsRequired(true);
        }
    }
}
