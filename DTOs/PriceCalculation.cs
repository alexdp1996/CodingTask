namespace DTOs
{
    public class PriceCalculation
    {
        public decimal DesiredBtc { get; set; }

        public decimal ExpectedBtc { get; set; }

        public decimal ExpectedPrice { get; set; }

        public List<Order> PriceDetails { get; set; }
    }
}
