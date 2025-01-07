namespace Integrations.Models
{
    public class BitstampOrderBook
    {
        public long Timestamp { get; set; }

        public long Microtimestamp { get; set; }

        public IEnumerable<decimal[]> Bids { get; set; }

        public IEnumerable<decimal[]> Asks { get; set; }
    }
}
