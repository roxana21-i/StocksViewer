using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;

namespace StockTests
{
    public class StockServiceTests
    {
        private readonly IBuyOrderCreatorService _buyOrderCreatorService;
        private readonly IBuyOrderGetterService _buyOrderGetterService;
        private readonly ISellOrderCreatorService _sellOrderCreatorService;
        private readonly ISellOrderGetterService _sellOrderGetterService;

        private readonly IStocksRepository _stocksRepository;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;

        private readonly IFixture _fixture;

        public StockServiceTests() {

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _buyOrderCreatorService = new BuyOrderCreatorService(_stocksRepository);
            _buyOrderGetterService = new BuyOrderGetterService(_stocksRepository);
            _sellOrderCreatorService = new SellOrderCreatorService(_stocksRepository);
            _sellOrderGetterService = new SellOrderGetterService(_stocksRepository);

            _fixture = new Fixture();
        }

        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException
        [Fact]
        public async Task CreateBuyOrder_NullRequest_ToBeArgumentNullException()
        {
            BuyOrderRequest? request = null;

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_SmallOrderQuantity_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt16(0))
                .Create();

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_BigOrderQuantity_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(100001))
                .Create();

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_SmallOrderPric_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                 .With(temp => temp.Price, 0)
                 .Create();

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_BigOrderPrice_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                 .With(temp => temp.Price, 10001)
                 .Create();

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                 .With(temp => temp.Price, 500)
                 .With(temp => temp.Quantity, Convert.ToUInt32(50))
                 .With(temp => temp.StockSymbol, null as string)
                 .Create();

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_InvalidDate_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                 .With(temp => temp.Price, 500)
                 .With(temp => temp.Quantity, Convert.ToUInt32(50))
                 .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                 .Create();

            Func<Task> action = async () => {
                await _buyOrderCreatorService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                 .With(temp => temp.Price, 500)
                 .With(temp => temp.Quantity, Convert.ToUInt32(50))
                 .Create();

            BuyOrder orderExpected = _fixture.Build<BuyOrder>()
                .With(temp => temp.Price, 500)
                .With(temp => temp.Quantity, Convert.ToUInt32(50))
                .Create();

            BuyOrderResponse responseExpected = orderExpected.ToBuyOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(orderExpected);

            BuyOrderResponse responseFromAdd = await _buyOrderCreatorService.CreateBuyOrder(request);
            responseExpected.BuyOrderID = responseFromAdd.BuyOrderID;

            responseFromAdd.BuyOrderID.Should().NotBe(Guid.Empty);
            responseFromAdd.Should().Be(responseExpected);
        }
        #endregion

        #region CreateSellOrder
        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullRequest_ToBeArgumentNullException()
        {
            SellOrderRequest? request = null;

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SmallOrderQuantity_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(0))
                .With(temp => temp.Price, 50)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_BigOrderQuantity_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(100001))
                .With(temp => temp.Price, 50)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SmallOrderPrice_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 0)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_BigOrderPrice_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 10001)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 150)
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_InvalidDate_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 150)
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrderCreatorService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidData_ToBeSuccessful()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 150)
                .Create();

            SellOrder expectedOrder = _fixture.Build<SellOrder>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 150)
                .Create();

            SellOrderResponse expectedResponse = expectedOrder.ToSellOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(expectedOrder);

            SellOrderResponse responseFromAdd = await _sellOrderCreatorService.CreateSellOrder(request);
            expectedResponse.SellOrderID = responseFromAdd.SellOrderID;

            responseFromAdd.SellOrderID.Should().NotBe(Guid.Empty);
            responseFromAdd.Should().Be(expectedResponse);
        }
        #endregion

        #region GetBuyOrders
        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetBuyOrders_EmptyList()
        {
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());

            List<BuyOrderResponse> allBuyOrders = await _buyOrderGetterService.GetBuyOrders();

            allBuyOrders.Should().BeEmpty();
        }

        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetBuyOrders_AddValidBuyOrders_ToBeSuccessful()
        {
            List<BuyOrder> buyOrders = new List<BuyOrder>()
            {
                 _fixture.Build<BuyOrder>()
                .With(temp => temp.Price, 500)
                .With(temp => temp.Quantity, Convert.ToUInt32(50))
                .Create(),
                  _fixture.Build<BuyOrder>()
                .With(temp => temp.Price, 700)
                .With(temp => temp.Quantity, Convert.ToUInt32(100))
                .Create()
            };

            List<BuyOrderResponse> expectedResponses = 
                buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> allBuyOrders = await _buyOrderGetterService.GetBuyOrders();

            allBuyOrders.Should().BeEquivalentTo(expectedResponses);

            
        }
        #endregion

        #region GetSellOrders
        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetSellOrders_EmptyList()
        {
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());

            List<SellOrderResponse> allSellOrders = await _sellOrderGetterService.GetSellOrders();

            allSellOrders.Should().BeEmpty();
        }

        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetSellOrders_AddValidSellOrders_ToBeSuccessful()
        {
            List<SellOrder> sellOrders = new List<SellOrder>()
            {
                _fixture.Build<SellOrder>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 150)
                .Create(),
                _fixture.Build<SellOrder>()
                .With(temp => temp.Quantity, Convert.ToUInt32(10))
                .With(temp => temp.Price, 150)
                .Create(),
            };

            List<SellOrderResponse> expectedResponses = 
                sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);

            List<SellOrderResponse> allSellOrders = await _sellOrderGetterService.GetSellOrders();

            allSellOrders.Should().BeEquivalentTo(expectedResponses);
        }
        #endregion
    }
}