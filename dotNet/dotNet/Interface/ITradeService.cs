using dotNet.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface ITradeService
    {
        public Task InsertDataToDB(string startDate, string endDate);

        public Task<List<TradeRespServiceModel>> GetDataFromDB();

        public Task DeleteData(int id);

    }
}
