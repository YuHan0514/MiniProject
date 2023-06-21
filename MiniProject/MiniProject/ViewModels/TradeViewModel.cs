﻿using System;

namespace MiniProject.ViewModels
{
    public class TradeViewModel
    {
        public int Id { get; set; }
        public DateTime TradeDate { get; set; }
        public string StockId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Volume { get; set; }
        public float Fee { get; set; }
        public float? Price { get; set; }
        public int LendingPeriod { get; set; }
        public DateTime ReturnDate => TradeDate.AddDays(LendingPeriod);
        public int Status { get; set; }
    }
}
