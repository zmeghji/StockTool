using AlphaVantageClient.ApiServices;
using Data;
using Data.Models;
using StockToolServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockToolConsole
{
    public interface IRunReportCommand
    {
        Task Execute(string filePath, DateTime event1Start, DateTime event2Start);
    }
    public class RunReportCommand : IRunReportCommand
    {
        private readonly IYearlyStatsService _yearlyStatsService;
        private readonly IQuoteApiService _quoteApiService;
        private readonly IDailyTimeSeriesApiService _dailyTimeSeriesApiService;
        private readonly IStatisticsService _statisticsService;
        private readonly StockReportContext _stockReportContext;
        public RunReportCommand(
            IYearlyStatsService yearlyStatsService,
            IQuoteApiService quoteApiService,
            IDailyTimeSeriesApiService dailyTimeSeriesApiService,
            IStatisticsService statisticsService,
            StockReportContext stockReportContext)
        {
            _yearlyStatsService = yearlyStatsService;
            _quoteApiService = quoteApiService;
            _dailyTimeSeriesApiService = dailyTimeSeriesApiService;
            _statisticsService = statisticsService;
            _stockReportContext = stockReportContext;
        }
        public async Task Execute(string filePath, DateTime event1Start, DateTime event2Start)
        {
            var report = new Report();
            report.Id = Guid.NewGuid();
            report.ReportDate = DateTime.Now.Date;
            report.Event1StartDate = event1Start;
            report.Event2StartDate = event2Start;

            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                try
                {
                    var line = reader.ReadLine();
                    var columns = line.Split(',');
                    var stockReport = await CreateStockReportModel(
                        symbol: columns[InputFileConstants.StockSymbolColumnIndex],
                        companyName: columns[InputFileConstants.CompanyNameColumnIndex],
                        category: columns[InputFileConstants.CategoryColumnIndex],
                        event1Start: event1Start,
                        event2Start: event2Start
                        );
                    report.StockReports.Add(stockReport);
                    Console.WriteLine($"Processed {columns[InputFileConstants.StockSymbolColumnIndex]}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Failed To Process Row");
                    Console.WriteLine($"Error Message: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }
            _stockReportContext.Reports.Add(report);
            _stockReportContext.SaveChanges();
        }

        private async Task<StockReport> CreateStockReportModel(
            string symbol, 
            string companyName, 
            string category,
            DateTime event1Start,
            DateTime event2Start)
        {
            var stockReport = new StockReport
            {
                CompanyName = companyName,
                Symbol = symbol,
                Category = category
            };
            stockReport.CurrentPrice =
                (await _quoteApiService.Get(symbol)).Quote.Price;

            var dailyTimeSeries = await _dailyTimeSeriesApiService.Get(symbol);
            stockReport.ChangeFromEvent1ToEvent2 = _statisticsService.Difference(dailyTimeSeries, event1Start, event2Start.AddDays(-1));
            stockReport.ChangeFromEvent2ToReportDate = _statisticsService.Difference(dailyTimeSeries, event2Start, DateTime.Now.Date);

            
                var yearlyStats = await _yearlyStatsService.GetMinAndAvg(
                    symbol,
                    event1Start,
                    new List<int> {1,2,3,4,5});
            stockReport.YearlyStats = yearlyStats.Select(y => new StockReportYearlyStats
            {
                NumberOfYearsPriorToEvent1 = y.NumberOfYears,
                Min = y.Min,
                Avg = y.Avg
            }).ToList();
            return stockReport;
        }
    }
}
