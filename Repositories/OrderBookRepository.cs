using DTOs;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
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
                            Type = OrderType.Bid,
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

        public async Task<(int, IEnumerable<OrderBook>)> GetAsync(int page, int perPage, CancellationToken cancellationToken)
        {
            var count = await _databaseContext.OrderBooks.CountAsync(cancellationToken);
            var entities = await _databaseContext.OrderBooks.OrderByDescending(x => x.Timestamp).Skip(page * perPage).Take(perPage).ToListAsync(cancellationToken);
            
            var dtos = entities
                .Select(x => new OrderBook
                {
                    Timestamp = x.Timestamp,
                }).ToList();

            return (count, dtos);
        }

        public async Task<OrderBook> GetByTimestampAsync(DateTimeOffset timestamp, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.OrderBooks
                .Include(x=> x.Orders)
                .FirstOrDefaultAsync(x => x.Timestamp == timestamp, cancellationToken);

            OrderBook dto = null;

            if (entity != null)
            {
                dto = new OrderBook
                {
                    Timestamp = timestamp,
                };

                dto.Asks = entity.Orders.Where(x => x.Type == OrderType.Ask).Select(x => new Order 
                {
                    Amount = x.Amount,
                    Price = x.Price,
                }).OrderBy(x => x.Price).ToList();

                dto.Bids = entity.Orders.Where(x => x.Type == OrderType.Ask).Select(x => new Order
                {
                    Amount = x.Amount,
                    Price = x.Price,
                }).OrderBy(x => x.Price).ToList();
            }

            return dto;
        }
    }
}
