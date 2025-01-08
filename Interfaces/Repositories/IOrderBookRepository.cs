using DTOs;

namespace Interfaces.Repositories
{
    public interface IOrderBookRepository
    {
        Task SaveAsync(OrderBook book, CancellationToken cancellationToken);
    }
}
