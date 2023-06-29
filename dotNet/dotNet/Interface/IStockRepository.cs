using dotNet.DBModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> JoinAllTable(int startIndex, int pageSize, string sortColumn, DateTime startDate, DateTime endDate, string tradeType, string stockId,string sortDirection);
        public Task<int> GetJoinAllTableCount(DateTime startDate, DateTime endDate, string tradeType, string stockId);
        public Task<JoinTable> JoinAllTableById(int id);
    }
}
