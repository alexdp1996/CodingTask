using DTOs;
using Interfaces.Integrations;
using Interfaces.Repositories;
using Interfaces.Services;
using OrderBookEM = Repositories.Models.OrderBook;
using OrderEM = Repositories.Models.Order;
using OrderType = Repositories.Models.OrderType;

namespace Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly IOrderBookProvider _orderBookProvider;
        private readonly IOrderBookRepository _orderBookRepository;

        public OrderBookService(IOrderBookProvider orderBookProvider, IOrderBookRepository orderBookRepository)
        {
            _orderBookProvider = orderBookProvider;
            _orderBookRepository = orderBookRepository;
        }

        public async Task<OrderBook> ReadAndSaveAsync(CancellationToken cancellationToken)
        {
            var orderBook = await _orderBookProvider.GetAsync(cancellationToken);

            var orderEnities = orderBook.Asks
                .Select(x => 
                    new OrderEM
                    {
                        Amount = x.Amount,
                        Price = x.Price,
                        Type = OrderType.Ask,
                    })
                .Concat(
                    orderBook.Bids.Select(x =>
                        new OrderEM
                        {
                            Amount = x.Amount,
                            Price = x.Price,
                            Type = OrderType.Ask,
                        })
                    ).ToList();

            var entity = new OrderBookEM
            {
                Timestamp = orderBook.Timestamp,
                Orders = orderEnities,
            };

            await _orderBookRepository.SaveAsync(entity, cancellationToken);

            return orderBook;
        }
    }
}
