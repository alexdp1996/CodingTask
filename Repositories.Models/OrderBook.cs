namespace Repositories.Models
{
    public class OrderBook
    {
        public Guid Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
