using DTOs;

namespace Interfaces.Integrations
{
    public interface IOrderBookProvider
    {
        Task<OrderBook> GetAsync(CancellationToken cancellationToken);
    }
}
