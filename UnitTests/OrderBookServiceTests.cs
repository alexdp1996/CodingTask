using DTOs;
using Interfaces.Integrations;
using Interfaces.Repositories;
using Moq;
using Services;

namespace UnitTests
{
    [TestClass]
    public sealed class OrderBookServiceTests
    {
        private readonly OrderBookService _service;
        private readonly Mock<IOrderBookRepository> _repositoryMock;
        private readonly Mock<IOrderBookProvider> _orderBookProviderMock;

        public OrderBookServiceTests()
        {
            _repositoryMock = new Mock<IOrderBookRepository>();
            _orderBookProviderMock = new Mock<IOrderBookProvider>();
            _service = new OrderBookService(_orderBookProviderMock.Object, _repositoryMock.Object);
        }

        [TestMethod]
        public async Task OrderBookService_EnoughIn1Bid_ReturnsFullCalculation()
        {
            // Arrange
            _orderBookProviderMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OrderBook
                {
                    Bids = new List<Order>
                    {
                        new Order
                        {
                            Amount = 1000,
                            Price = 60000,
                        }
                    }
                });

            // Act
            var calculation = await _service.CalculatePriceAsync(20, CancellationToken.None);

            // Assert
            
            var priceDetail = calculation.PriceDetails.First();

            Assert.IsNotNull(calculation);
            Assert.AreEqual(20, calculation.DesiredAmount);
            Assert.AreEqual(20, calculation.ExpectedAmount);
            Assert.AreEqual(1200000, calculation.ExpectedPrice);
            Assert.AreEqual(1, calculation.PriceDetails.Count);
            Assert.AreEqual(60000, priceDetail.Price);
            Assert.AreEqual(1200000, priceDetail.TotalPrice);
            Assert.AreEqual(20, priceDetail.Amount);
        }

        [TestMethod]
        public async Task OrderBookService_EnoughIn2Bids_ReturnsFullCalculation()
        {
            // Arrange
            _orderBookProviderMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OrderBook
                {
                    Bids = new List<Order>
                    {
                        new Order
                        {
                            Amount = 2,
                            Price = 60000,
                        },
                        new Order
                        {
                            Amount = 1000,
                            Price = 65000,
                        }
                    },
                });

            // Act
            var calculation = await _service.CalculatePriceAsync(20, CancellationToken.None);

            // Assert

            var priceDetail1 = calculation.PriceDetails[0];
            var priceDetail2 = calculation.PriceDetails[1];

            Assert.IsNotNull(calculation);
            Assert.AreEqual(20, calculation.DesiredAmount);
            Assert.AreEqual(20, calculation.ExpectedAmount);
            Assert.AreEqual(1290000, calculation.ExpectedPrice);
            Assert.AreEqual(2, calculation.PriceDetails.Count);

            Assert.AreEqual(60000, priceDetail1.Price);
            Assert.AreEqual(2, priceDetail1.Amount);
            Assert.AreEqual(120000, priceDetail1.TotalPrice);

            Assert.AreEqual(65000, priceDetail2.Price);
            Assert.AreEqual(18, priceDetail2.Amount);
            Assert.AreEqual(1170000, priceDetail2.TotalPrice);
        }

        [TestMethod]
        public async Task OrderBookService_NotEnoughInBid_ReturnsPartialCalculation()
        {
            // Arrange
            _orderBookProviderMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OrderBook
                {
                    Bids = new List<Order>
                    {
                        new Order
                        {
                            Amount = 15,
                            Price = 60000,
                        },
                    },
                });

            // Act
            var calculation = await _service.CalculatePriceAsync(20, CancellationToken.None);

            // Assert

            var priceDetail = calculation.PriceDetails.First();

            Assert.IsNotNull(calculation);
            Assert.AreEqual(20, calculation.DesiredAmount);
            Assert.AreEqual(15, calculation.ExpectedAmount);
            Assert.AreEqual(900000, calculation.ExpectedPrice);
            Assert.AreEqual(1, calculation.PriceDetails.Count);

            Assert.AreEqual(60000, priceDetail.Price);
            Assert.AreEqual(15, priceDetail.Amount);
            Assert.AreEqual(900000, priceDetail.TotalPrice);
        }

        [TestMethod]
        public async Task OrderBookService_NotEnoughIn2Bids_ReturnsPartialCalculation()
        {
            // Arrange
            _orderBookProviderMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OrderBook
                {
                    Bids = new List<Order>
                    {
                        new Order
                        {
                            Amount = 1,
                            Price = 60000,
                        },
                        new Order
                        {
                            Amount = 2,
                            Price = 65000,
                        },
                    },
                });

            // Act
            var calculation = await _service.CalculatePriceAsync(20, CancellationToken.None);

            // Assert

            var priceDetail1 = calculation.PriceDetails[0];
            var priceDetail2 = calculation.PriceDetails[1];

            Assert.IsNotNull(calculation);
            Assert.AreEqual(20, calculation.DesiredAmount);
            Assert.AreEqual(3, calculation.ExpectedAmount);
            Assert.AreEqual(190000, calculation.ExpectedPrice);
            Assert.AreEqual(2, calculation.PriceDetails.Count);

            Assert.AreEqual(60000, priceDetail1.Price);
            Assert.AreEqual(1, priceDetail1.Amount);
            Assert.AreEqual(60000, priceDetail1.TotalPrice);

            Assert.AreEqual(65000, priceDetail2.Price);
            Assert.AreEqual(2, priceDetail2.Amount);
            Assert.AreEqual(130000, priceDetail2.TotalPrice);
        }
    }
}
