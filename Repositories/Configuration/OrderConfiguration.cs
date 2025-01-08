using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Models;

namespace Repositories.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<OrderBook>()
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.OrderBookId);

            builder.Property(x => x.Type)
            .HasConversion(
                x => x.ToString(),
                x => Enum.Parse<OrderType>(x));
        }
    }
}
