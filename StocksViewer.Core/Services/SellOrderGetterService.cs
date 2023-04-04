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
    public class SellOrderGetterService : ISellOrderGetterService
    {
        private readonly IStocksRepository _stocksRepository;

        public SellOrderGetterService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellorders = await _stocksRepository.GetSellOrders();
            return sellorders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }

    }
}
