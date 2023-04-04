using ServiceContracts;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class FinnhubSearchService : IFinnhubSearchService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubGetterService> _logger;

        public FinnhubSearchService(IFinnhubRepository finnhubRepository, ILogger<FinnhubGetterService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            return await _finnhubRepository.SearchStocks(stockSymbolToSearch);
        }
    }
}
