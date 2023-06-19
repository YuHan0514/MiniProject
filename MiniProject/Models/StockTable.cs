using System;
using System.Collections.Generic;

#nullable disable

namespace MiniProject.Models
{
    public partial class StockTable
    {
        public StockTable()
        {
            ClosingPriceTables = new HashSet<ClosingPriceTable>();
            TradeTables = new HashSet<TradeTable>();
        }

        public string StockId { get; set; }
        public string Name { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual ICollection<ClosingPriceTable> ClosingPriceTables { get; set; }
        public virtual ICollection<TradeTable> TradeTables { get; set; }
    }
}
