using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.ViewModels
{
    public class TradeViewModel
    {
        public int Id { get; set; }
        public string StockId { get; set; }
        public string Name { get; set; }
        public DateTime TradeDate { get; set; }
        public string Type { get; set; }
        public int Volume { get; set; }
        public float Fee { get; set; }
        public int LendingPeriod { get; set; }
        public DateTime ReturnDate => TradeDate.AddDays(LendingPeriod);
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
