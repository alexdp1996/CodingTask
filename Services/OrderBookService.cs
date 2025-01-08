using DTOs;
using Interfaces.Integrations;
using Interfaces.Repositories;
using Interfaces.Services;

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

            await _orderBookRepository.SaveAsync(orderBook, cancellationToken);

            return orderBook;
        }
    }
}
