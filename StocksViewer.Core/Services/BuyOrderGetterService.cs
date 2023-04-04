using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BuyOrderGetterService : IBuyOrderGetterService
    {
        private readonly IStocksRepository _stocksRepository;

        public BuyOrderGetterService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> GetBuyOrderByID(Guid buyOrderID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyorders = await _stocksRepository.GetBuyOrders();
            return buyorders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

    }
}
