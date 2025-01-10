namespace DTOs
{
    public class CalculationOrder : Order
    {
        public decimal TotalPrice => Amount * Price;
    }
}
