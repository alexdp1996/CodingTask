using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Models;

namespace Repositories.Configuration
{
    public class OrderBookConfiguration : IEntityTypeConfiguration<OrderBook>
    {
        public void Configure(EntityTypeBuilder<OrderBook> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Orders)
                .WithOne()
                .HasForeignKey(x => x.OrderBookId);
        }
    }
}
