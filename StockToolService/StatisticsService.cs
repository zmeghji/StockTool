using AlphaVantageClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace StockToolServices
{
    public interface IStatisticsService
    {
        double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries);
        double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to);

    }
    public class StatisticsService : IStatisticsService
    {
        
        public double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries)
        {
            return Min(monthlyTimeSeries.MonthlyTimeSeries.Values.ToList());
        }

        public double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {

            var filteredMonthlyTimeSeries = monthlyTimeSeries.MonthlyTimeSeries
                    .Where(m => DateTime.Parse(m.Key) >= from && DateTime.Parse(m.Key) <= to)
                    .Select(m => m.Value).ToList();

            return Min(filteredMonthlyTimeSeries);
        }
        private double Min(List<MonthQuoteApiModel> monthQuotes)
        {
            return monthQuotes.Min(m => m.Low);
        }
    }
}
