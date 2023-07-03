using dotNet.ServiceModels;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockService
    {
        public Task<StockRespServiceModel> GetStockInfo(StockServiceModel stock);
    }
}
