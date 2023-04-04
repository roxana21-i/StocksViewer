using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using StocksApp.Models;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubGetterService _finnhubService;

        public SelectedStockViewComponent(IFinnhubGetterService finnhubService)
        {
            _finnhubService = finnhubService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stock)
        {
            Dictionary<string, object>? companyProfile = null;

            if (stock != null)
            {
                companyProfile = await _finnhubService.GetCompanyProfile(stock);
                Dictionary<string, object>? priceQuote = await _finnhubService.GetStockPriceQuote(stock);

                if (companyProfile != null && priceQuote != null) 
                {
                    companyProfile.Add("price", priceQuote["c"]);
                }
            }

            if (companyProfile != null && companyProfile.ContainsKey("logo"))
                return View(companyProfile);
            else
                return Content("");
        }
    }
}
