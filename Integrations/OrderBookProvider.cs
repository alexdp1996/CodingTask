using DTOs;
using Integrations.Api;
using Interfaces.Integrations;

namespace Integrations
{
    public class OrderBookProvider : IOrderBookProvider
    {
        private readonly IBitstampApi _bitstampApi;

        public OrderBookProvider(IBitstampApi bitstampApi)
        {
            _bitstampApi = bitstampApi;
        }

        public async Task<OrderBook> GetAsync(CancellationToken cancellationToken)
        {
            var response = await _bitstampApi.GetOrderBookAsync(cancellationToken);
            await response.EnsureSuccessfulAsync();
            var content = response.Content;

            var asks = content.Asks.Select(x => new Order
            {
                Price = x[0],
                Amount = x[1],
            });

            var bids = content.Asks.Select(x => new Order
            {
                Price = x[0],
                Amount = x[1],
            });

            var orderBook = new OrderBook
            {
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(content.Timestamp),
                Asks = asks,
                Bids = bids,
            };

            return orderBook;
        }
    }
}
