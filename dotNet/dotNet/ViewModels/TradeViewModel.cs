using System;

namespace dotNet.ViewModels
{
    public class TradeViewModel
    {
        public int pageIndex { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string tradeType { get; set; }
        public string stockId { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }
}
