using DTOs;
using Interfaces.Integrations;
using Interfaces.Repositories;
using Interfaces.Services;
using System.Diagnostics;

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
                DesiredAmount = amountToBuy,
                PriceDetails = new List<CalculationOrder>(),
            };

            foreach (var order in orderBook.Bids)
            {
                var calculationOrder = amountToBuy >= order.Amount
                    ? new CalculationOrder
                    {
                        Amount = order.Amount,
                        Price = order.Price,
                    }
                    : new CalculationOrder
                    {
                    Amount = amountToBuy,
                        Price = order.Price,
                    };

                calculation.ExpectedAmount += calculationOrder.Amount;
                calculation.ExpectedPrice += calculationOrder.TotalPrice;
                amountToBuy -= calculationOrder.Amount;

                calculation.PriceDetails.Add(calculationOrder);

                if (amountToBuy == 0)
                    break;
            }

            return calculation;
        }
    }
}
