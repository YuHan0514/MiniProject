using System.Collections.Generic;

namespace dotNet.ViewModels
{
    public class TradeQueryRespViewModel
    {
        public List<TradeRespViewModel> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
    }
}
