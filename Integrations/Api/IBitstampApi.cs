using Integrations.Models;
using Refit;

namespace Integrations.Api
{
    public interface IBitstampApi
    {
        [Get("/api/v2/order_book/btceur")]
        public Task<ApiResponse<BitstampOrderBook>> GetOrderBookAsync(CancellationToken cancellationToken);
    }
}
