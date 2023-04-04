using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubGetterService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public StocksController(IFinnhubGetterService finnhubService, IOptions<TradingOptions> options)
        {
            _finnhubService = finnhubService;
            _tradingOptions = options.Value;
        }

        [Route("[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();

            List<StockTrade> stocks = new List<StockTrade>();

            if (stocksDictionary != null)
            {
                if (showAll == false && _tradingOptions.Top25PopularStocks != null) 
                {
                    string[]? popularStocks = _tradingOptions.Top25PopularStocks.Split(',');

                    if (popularStocks != null)
                    {
                        stocksDictionary = stocksDictionary.Where(
                            temp => popularStocks.Contains(Convert.ToString(temp["symbol"]))).ToList();
                    }
                }
            }

            stocks = stocksDictionary.Select(temp => new StockTrade()
            {
                StockSymbol = temp["symbol"],
                StockName = temp["description"]
            }).ToList();

            ViewBag.Stock = stock;
            return View(stocks);
        }
    }
}
