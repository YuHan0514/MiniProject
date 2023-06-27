using System.Collections.Generic;

namespace dotNet.Models
{
    public class TwseTable
    {
        public string stat { get; set; }
        public object Params { get; set; }
        public string title { get; set; }
        public object fields { get; set; }
        public List<List<object>> data { get; set; }
        public object notes { get; set; }
    }
}
