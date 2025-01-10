using DTOs;

namespace Interfaces.Repositories
{
    public interface IOrderBookRepository
    {
        Task SaveAsync(OrderBook book, CancellationToken cancellationToken);

        Task<(int, IEnumerable<OrderBook>)> GetAsync(int page, int perPage, CancellationToken cancellationToken);

        Task<OrderBook> GetByTimestampAsync(DateTimeOffset timestamp, CancellationToken cancellationToken);
    }
}
