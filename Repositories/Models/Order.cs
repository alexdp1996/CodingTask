namespace Repositories.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid OrderBookId { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public OrderType Type { get; set; }
    }
}
