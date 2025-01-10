namespace DTOs
{
    public class PriceCalculation
    {
        public decimal DesiredAmount { get; set; }

        public decimal ExpectedAmount { get; set; }

        public decimal ExpectedPrice { get; set; }

        public List<CalculationOrder> PriceDetails { get; set; }
    }
}
