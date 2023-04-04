using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ISellOrderGetterService
    {
        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'.
        /// </summary>
        /// <returns>Returns the existing list of sell orders retrieved from database table called 'SellOrders'.</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
