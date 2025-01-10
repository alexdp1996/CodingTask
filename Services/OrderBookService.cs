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

        public OrderBookService(IOrderBookProvider orderBookProvider, IOrderBookRepository orderBookRepository
            )
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

        public async Task<(int, IEnumerable<OrderBook>)> GetAuditRecordsAsync(int pageNumber, int perPage, CancellationToken cancellationToken)
        {
            return await _orderBookRepository.GetAsync(pageNumber, perPage, cancellationToken);
        }

        public async Task<OrderBook> GetAuditRecordByTimestampAsync(DateTimeOffset timestamp, CancellationToken cancellationToken)
        {
            return await _orderBookRepository.GetByTimestampAsync(timestamp, cancellationToken);
        }

        public async Task<PriceCalculation> CalculatePriceAsync(decimal amountToBuy, CancellationToken cancellationToken)
        {
            var orderBook = await ReadAndSaveAsync(cancellationToken);

            var calculation = new PriceCalculation
            {
                DesiredBtc = amountToBuy,
                PriceDetails = new List<Order>(),
            };

            foreach (var order in orderBook.Bids)
            {
                if (amountToBuy == 0)
                    break;

                if (amountToBuy >= order.Amount)
                {
                    calculation.PriceDetails.Add(order);

                    calculation.ExpectedAmount += order.Amount;
                    calculation.ExpectedPrice += order.Amount * order.Price;
                    
                    amountToBuy -= order.Amount;

                    continue;
                }

                if (amountToBuy < order.Amount)
                {
                    calculation.PriceDetails.Add(new Order { Amount = amountToBuy, Price = order.Price });
                    
                    calculation.ExpectedAmount += amountToBuy;
                    calculation.ExpectedPrice += amountToBuy * order.Price;

                    amountToBuy = 0;
                }
            }

            return calculation;
        }
    }
}
