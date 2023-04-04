using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StockTests
{
    public class StocksControllerTest
    {
        private readonly IFinnhubGetterService _finnhubService;
        private readonly Mock<IFinnhubGetterService> _finnhubServiceMock;
        private readonly IFixture _fixture;

        public StocksControllerTest()
        {
            _fixture = new Fixture();

            _finnhubServiceMock = new Mock<IFinnhubGetterService>();
            _finnhubService = _finnhubServiceMock.Object;
        }

        #region Explore
        [Fact]
        public async Task Explore_ToReturnView()
        {
            List<Dictionary<string, string>>? stocksDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");

            IOptions<TradingOptions> tradingOptions = Options.Create
                (new TradingOptions() 
                {
                    DefaultStockSymbol = "MSFT", 
                    Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO" 
                });


            _finnhubServiceMock.Setup(temp => temp.GetStocks()).ReturnsAsync(stocksDictionary);

            List<StockTrade>? expectedDictionary = stocksDictionary
                .Select(temp => new StockTrade()
                {
                    StockName = Convert.ToString(temp["description"]),
                    StockSymbol = Convert.ToString(temp["symbol"])
                }).ToList();

            StocksController stockController = new StocksController(_finnhubService, tradingOptions);

            IActionResult result = await stockController.Explore(null, false);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<StockTrade>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(expectedDictionary);
        }

        #endregion
    }
}
