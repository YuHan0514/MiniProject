using System.Collections.Generic;

namespace dotNet.ServiceModels
{
    public class TradeQueryRespServiceModel
    {
        public List<TradeRespServiceModel> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
