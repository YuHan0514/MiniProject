using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Service.Interface
{
    public interface ITradeService
    {
        public Task<TradeRespServiceModel> Get();
    }
}
