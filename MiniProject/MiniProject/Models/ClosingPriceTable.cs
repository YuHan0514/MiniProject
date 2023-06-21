using System;

#nullable disable

namespace MiniProject.Models
{
    public partial class ClosingPriceTable
    {
        public int Id { get; set; }
        public string StockId { get; set; }
        public DateTime TradeDate { get; set; }
        public double? Price { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual StockTable Stock { get; set; }
    }
}
