namespace ServiceContracts
{
    public interface IFinnhubGetterService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol);
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}
