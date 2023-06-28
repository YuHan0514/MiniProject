using dotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> JoinAllTable(int startIndex, int pageSize, string sortColumn, string startDate, string endDate, string tradeType, string stockId,string sortDirection);
        public Task<int> GetJoinAllTableCount(int startIndex, int pageSize, string sortColumn, string startDate, string endDate, string tradeType, string stockId,string sortDirection);
        public Task<JoinTable> JoinAllTableById(int id);
    }
}
