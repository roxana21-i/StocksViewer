using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp;
using StocksApp.Models;
using Services;
using ServiceContracts.DTO;
using Entities;
using ServiceContracts;
using Rotativa.AspNetCore;
using StocksApp.Filters.ActionFilters;

namespace StocksApp.Controllers
{
    [Route("trade")]
    public class TradeController : Controller
    {
        private readonly IFinnhubGetterService _finnhubService;
        private readonly IBuyOrderCreatorService _buyOrderCreatorService;
        private readonly IBuyOrderGetterService _buyOrderGetterService;
        private readonly ISellOrderCreatorService _sellOrderCreatorService;
        private readonly ISellOrderGetterService _sellOrderGetterService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(
            IFinnhubGetterService finnhubService, IOptions<TradingOptions> options, 
            IConfiguration configuration, 
            IBuyOrderCreatorService buyOrderCreatorService, 
            IBuyOrderGetterService buyOrderGetterService,
            ISellOrderCreatorService sellOrderCreatorService,
            ISellOrderGetterService sellOrderGetterService,
            ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _tradingOptions = options;
            _configuration = configuration;
            _buyOrderCreatorService = buyOrderCreatorService;
            _buyOrderGetterService = buyOrderGetterService;
            _sellOrderCreatorService = sellOrderCreatorService;
            _sellOrderGetterService = sellOrderGetterService;
            _logger = logger;
        }

        [Route("/")]
        [Route("[action]/{stockSymbol?}")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            _logger.LogInformation("Index Action of TradeController started");
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }
            if (stockSymbol == null)
            {
                stockSymbol = _tradingOptions.Value.DefaultStockSymbol;
            }
            Dictionary<string, object>? responseDictionaryStock = await _finnhubService.GetStockPriceQuote(stockSymbol);

            Dictionary<string, object>? responseDictionaryProfile = await _finnhubService.GetCompanyProfile(stockSymbol);

            StockTrade stockTrade = new StockTrade() {
                StockSymbol = stockSymbol,
                StockName = responseDictionaryProfile["name"].ToString(),
                Price = Convert.ToDouble(responseDictionaryStock["c"].ToString()),
                Quantity = 10,
            };

            //ViewBag.CompanyName = responseDictionaryProfile["name"].ToString();
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            ViewBag.CurrentTime = DateTime.Now.ToString("h:mm:ss tt");

            string buyordersJson = System.IO.File.ReadAllText("buyorders.json");
            List<BuyOrder> buyorders = System.Text.Json.JsonSerializer.Deserialize<List<BuyOrder>>(buyordersJson);

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            _logger.LogInformation("BuyOrder action of TradeController started");

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { 
                    StockName = orderRequest.StockName, 
                    Price = orderRequest.Price,
                    Quantity = orderRequest.Quantity, 
                    StockSymbol = orderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            BuyOrderResponse response = await _buyOrderCreatorService.CreateBuyOrder(orderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            _logger.LogInformation("SellOrder action of TradeController started");

            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(orderRequest);

            SellOrderResponse response = await _sellOrderCreatorService.CreateSellOrder(orderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new Orders();
            orders.BuyOrders = await _buyOrderGetterService.GetBuyOrders();
            orders.SellOrders = await _sellOrderGetterService.GetSellOrders();

            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            Orders orders = new Orders();
            orders.BuyOrders = await _buyOrderGetterService.GetBuyOrders();
            orders.SellOrders = await _sellOrderGetterService.GetSellOrders();

            return new ViewAsPdf("OrdersPDF", orders, ViewData){
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
