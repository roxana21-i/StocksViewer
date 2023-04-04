using ServiceContracts;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class FinnhubGetterService : IFinnhubGetterService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubGetterService> _logger;

        public FinnhubGetterService(IFinnhubRepository finnhubRepository, ILogger<FinnhubGetterService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("GetCompanyProfile of FinnhubService started");
            return await _finnhubRepository.GetCompanyProfile(stockSymbol);
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol)
        {
            _logger.LogInformation("GetStockPriceQuote of FinnhubService started");
            return await _finnhubRepository.GetStockPriceQuote(symbol);
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            return await _finnhubRepository.GetStocks();
        }
    }
}
