using dotNet.Service;
using dotNet.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface ITradeService
    {
        public Task<string> InsertDataToDB(string startDate, string endDate);

        public Task<List<TradeRespServiceModel>> GetDataFromDB();

        public Task<string> DeleteStockData(int id);

        public Task<TradeRespServiceModel> GetStockById(int id);

        public Task<string> UpdateStockById(TradeServiceModel stock);
    }
}
