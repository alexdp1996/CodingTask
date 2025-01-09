using DTOs;
using Integrations.Api;
using Interfaces.Integrations;
using System.Globalization;

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
                Price = decimal.Parse(x[0], CultureInfo.InvariantCulture),
                Amount = decimal.Parse(x[1], CultureInfo.InvariantCulture),
            });

            var bids = content.Asks.Select(x => new Order
            {
                Price = decimal.Parse(x[0], CultureInfo.InvariantCulture),
                Amount = decimal.Parse(x[1], CultureInfo.InvariantCulture),
            });

            var microTimestamp = long.Parse(content.Microtimestamp);
            var timeSpan = TimeSpan.FromMicroseconds(microTimestamp);

            var orderBook = new OrderBook
            {
                Timestamp = DateTimeOffset.UnixEpoch + timeSpan,
                Asks = asks.ToList(),
                Bids = bids.ToList(),
            };

            return orderBook;
        }
    }
}
