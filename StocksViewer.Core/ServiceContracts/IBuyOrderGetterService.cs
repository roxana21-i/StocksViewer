using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IBuyOrderGetterService
    {
        /// <summary>
        /// Gets a buy order matching the given id
        /// </summary>
        /// <param name="buyOrderID">ID to match</param>
        /// <returns>Returns the matching BuyOrder data as BuyOrderResponse</returns>
        Task<BuyOrderResponse> GetBuyOrderByID(Guid buyOrderID);
        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.
        /// </summary>
        /// <returns>Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
