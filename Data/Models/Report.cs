using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public DateTime Event1StartDate { get; set; }
        public DateTime Event2StartDate { get; set; }
        public DateTime ReportDate { get; set; }
        public ICollection<StockReport> StockReports { get; set; }= new List<StockReport>();
    }
}
