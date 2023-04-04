using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IBuyOrderCreatorService
    {
        /// <summary>
        /// Inserts a new buy order into the database table called 'BuyOrders'.
        /// </summary>
        /// <param name="buyOrderRequest">The BuyOrder to be added to the database</param>
        /// <returns>The created BuyOrder as a BuyOrderResponse</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
    }
}
