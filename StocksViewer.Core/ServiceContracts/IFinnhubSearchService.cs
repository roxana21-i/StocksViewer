namespace ServiceContracts
{
    public interface IFinnhubSearchService
    {
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}
