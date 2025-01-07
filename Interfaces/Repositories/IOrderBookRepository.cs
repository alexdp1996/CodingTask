using Repositories.Models;

namespace Interfaces.Repositories
{
    public interface IOrderBookRepository
    {
        Task SaveAsync(OrderBook book, CancellationToken cancellationToken);
    }
}
