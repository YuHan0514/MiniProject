using dotNet.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockService
    {
        public Task<StockRespServiceModel> GetStockInfo(string id, string tradeDate);
    }
}
