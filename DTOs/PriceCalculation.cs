namespace DTOs
{
    public class PriceCalculation
    {
        public decimal DesiredBtc { get; set; }

        public decimal ExpectedAmount { get; set; }

        public decimal ExpectedPrice { get; set; }

        public List<Order> PriceDetails { get; set; }
    }
}
