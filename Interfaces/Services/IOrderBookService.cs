using DTOs;

namespace Interfaces.Services
{
    public interface IOrderBookService
    {
        Task<OrderBook> ReadAndSaveAsync(CancellationToken cancellationToken);

        Task<(int, IEnumerable<OrderBook>)> GetAuditRecordsAsync(int pageNumber, int perPage, CancellationToken cancellationToken);

        Task<OrderBook> GetAuditRecordByTimestampAsync(DateTimeOffset timestamp, CancellationToken cancellationToken);

        Task<PriceCalculation> CalculatePriceAsync(decimal amountToBuy, CancellationToken cancellationToken);
    }
}
