using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class StockReport
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Category { get; set; }
        public double CurrentPrice { get; set; }
        public double ChangeFromEvent1ToEvent2 { get; set; }
        public double ChangeFromEvent2ToReportDate { get; set; }
        public ICollection<StockReportYearlyStats> YearlyStats { get; set; } = new List<StockReportYearlyStats>();
    }
}
