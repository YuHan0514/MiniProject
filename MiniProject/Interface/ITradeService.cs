using MiniProject.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Interface
{
    public interface ITradeService
    {
        public Task UpdateDB(string startDate, string endDate);

        public Task<List<TradeRespServiceModel>> GetList();

    }
}
