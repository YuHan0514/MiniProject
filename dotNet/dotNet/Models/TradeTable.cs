using System;

#nullable disable

namespace dotNet.Models
{
    public partial class TradeTable
    {
        public int Id { get; set; }
        public string StockId { get; set; }
        public DateTime TradeDate { get; set; }
        public string Type { get; set; }
        public int Volume { get; set; }
        public double Fee { get; set; }
        public int LendingPeriod { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual StockTable Stock { get; set; }
    }
}
