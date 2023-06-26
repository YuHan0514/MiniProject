using dotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface ITradeHelper
    {
        public Task<List<JoinTable>> GetDataFromURL(string startDate, string endDate);
    }
}
