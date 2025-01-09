namespace Integrations.Models
{
    public class BitstampOrderBook
    {
        public string Timestamp { get; set; }

        public string Microtimestamp { get; set; }

        public IEnumerable<string[]> Bids { get; set; }

        public IEnumerable<string[]> Asks { get; set; }
    }
}
