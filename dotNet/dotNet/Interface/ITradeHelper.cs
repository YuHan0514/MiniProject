using dotNet.DBModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface ITradeHelper
    {
        public Task<List<JoinTable>> GetStockListFromTwse(string startDate, DateTime endDate);
    }
}
