namespace DTOs
{
    public class OrderBook
    {
        public IEnumerable<Order> Bids { get; set; }

        public IEnumerable<Order> Asks { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
