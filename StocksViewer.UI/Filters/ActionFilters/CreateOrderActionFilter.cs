using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StocksApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;

                if (orderRequest != null)
                {
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    //re-validate the model object after updating the date
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        StockTrade stockTrade = new StockTrade()
                        {
                            StockName = orderRequest.StockName,
                            Price = orderRequest.Price,
                            Quantity = orderRequest.Quantity,
                            StockSymbol = orderRequest.StockSymbol
                        };
                        context.Result = tradeController.View(nameof(TradeController.Index), orderRequest);
                    }
                    else
                    {
                        await next();
                    }
                }
            }
            else
            {
                await next();
            }
        }
    }
}
