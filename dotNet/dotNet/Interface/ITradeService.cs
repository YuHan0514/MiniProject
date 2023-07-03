using dotNet.ServiceModels;
using System;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface ITradeService
    {
        public Task<TradeTwseRespServiceModel> InsertTwseDataToDB(DateTime endDate);

        public Task<TradeQueryRespServiceModel> GetStockListFromDB(TradeQueryServiceModel tradeQueryServiceModel);

        public Task DeleteStockByStatus(int id);

        public Task<TradeRespServiceModel> GetStockById(int id);

        public Task<TradeTwseRespServiceModel> UpdateTradeInfoById(TradeServiceModel stock);
    }
}
