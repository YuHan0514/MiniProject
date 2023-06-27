using System;

namespace dotNet.ViewModels
{
    public class TradeRespViewModel
    {
        public int Id { get; set; }
        public string TradeDate { get; set; }
        public string StockId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Volume { get; set; }
        public float Fee { get; set; }
        public float? Price { get; set; }
        public int LendingPeriod { get; set; }
        public string ReturnDate => DateTime.Parse(TradeDate).AddDays(LendingPeriod).ToString("yyyy-MM-dd");
        public int Status { get; set; }
    }
}
