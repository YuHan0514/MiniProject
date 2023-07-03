using System;

namespace dotNet.ServiceModels
{
    public class TradeQueryServiceModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string tradeType { get; set; }
        public string stockId { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }
}
