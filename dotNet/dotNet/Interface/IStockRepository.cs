using dotNet.DBModels;
using dotNet.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> JoinAndFilterAllTable(TradeQueryServiceModel tradeQueryServiceModel);
        public Task<int> GetJoinAndFilterAllTableCount(TradeQueryServiceModel tradeQueryServiceModel);
        public Task<TradeRespServiceModel> GetTradeInfoById(int id);
    }
}
