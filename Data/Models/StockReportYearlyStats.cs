using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class StockReportYearlyStats
    {
        public int NumberOfYearsPriorToEvent1 { get; set; }
        public double Min { get; set; }
        public double Avg { get; set; }
    }
}
