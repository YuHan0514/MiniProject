using dotNet.Service;
using dotNet.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface ITradeService
    {
        public Task<string> InsertTwseDataToDB(string startDate, string endDate);

        public Task<List<TradeRespServiceModel>> GetStockListFromDB(int pageIndex, int pageSize, string sortColumn, string startDate, string endDate, string tradeType, string stockId, string sortDirection);

        public Task<string> DeleteStockByStatus(int id);

        public Task<TradeRespServiceModel> GetStockById(int id);

        public Task<string> UpdateStockById(TradeServiceModel stock);
    }
}
