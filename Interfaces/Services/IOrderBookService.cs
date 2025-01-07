using DTOs;

namespace Interfaces.Services
{
    public interface IOrderBookService
    {
        Task<OrderBook> ReadAndSaveAsync(CancellationToken cancellationToken);
    }
}
