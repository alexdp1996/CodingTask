using DTOs;
using Interfaces.Repositories;
using OrderBookEM = Repositories.Models.OrderBook;
using OrderEM = Repositories.Models.Order;
using OrderType = Repositories.Models.OrderType;

namespace Repositories
{
    public class OrderBookRepository : IOrderBookRepository
    {
        private readonly DatabaseContext _databaseContext;

        public OrderBookRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task SaveAsync(OrderBook book, CancellationToken cancellationToken)
        {
            var orderEnities = book.Asks
            .Select(x =>
                    new OrderEM
                    {
                        Amount = x.Amount,
                        Price = x.Price,
                        Type = OrderType.Ask,
                    })
            .Concat(
                    book.Bids.Select(x =>
                        new OrderEM
                        {
                            Amount = x.Amount,
                            Price = x.Price,
                            Type = OrderType.Ask,
                        })
            ).ToList();

            var entity = new OrderBookEM
            {
                Timestamp = book.Timestamp,
                Orders = orderEnities,
            };

            await _databaseContext.OrderBooks.AddAsync(entity, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
