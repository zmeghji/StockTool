using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class StockReportContext : DbContext
    {
        public StockReportContext(DbContextOptions<StockReportContext> options) : base(options)
        {
        }
        public DbSet<Report> Reports { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Report>()
                .OwnsMany(r => r.StockReports)
                .OwnsMany(s => s.YearlyStats);
        }
    }
}
