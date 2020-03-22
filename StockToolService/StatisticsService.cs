using AlphaVantageClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace StockToolServices
{
    public interface IStatisticsService
    {
        double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to);

        double Avg(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to);

        double Difference(DailyTimeSeriesApiModel dailyTimeSeries, DateTime start, DateTime end);
    }
    public class StatisticsService : IStatisticsService
    {

        public double Avg(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {
            return Filter(monthlyTimeSeries, from, to).Average(m => m.Close);
        }

        public double Difference(DailyTimeSeriesApiModel dailyTimeSeries, DateTime start, DateTime end)
        {
            var startQuote = GetStartQuote(dailyTimeSeries, start);
            var endQuote = GetEndQuote(dailyTimeSeries, end);
            return endQuote.Close - startQuote.Open;
        }
        private DailyQuoteApiModel GetStartQuote(DailyTimeSeriesApiModel dailyTimeSeries, DateTime start)
        {
            var startQuote = dailyTimeSeries.DailyTimeSeries.SingleOrDefault(d => DateTime.Parse(d.Key) == start);
            if (startQuote.Value == null)
            {
                startQuote = dailyTimeSeries.DailyTimeSeries
                    .Where(d => DateTime.Parse(d.Key) > start)
                    .OrderByDescending(d => DateTime.Parse(d.Key))
                    .First();
            }
            return startQuote.Value;
        }
        private DailyQuoteApiModel GetEndQuote(DailyTimeSeriesApiModel dailyTimeSeries, DateTime end)
        {
            var startQuote = dailyTimeSeries.DailyTimeSeries.SingleOrDefault(d => DateTime.Parse(d.Key) == end);
            if (startQuote.Value == null)
            {
                startQuote = dailyTimeSeries.DailyTimeSeries
                    .Where(d => DateTime.Parse(d.Key) < end)
                    .OrderBy(d => DateTime.Parse(d.Key))
                    .Last();
            }
            return startQuote.Value;
        }
        public double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {
            return Filter(monthlyTimeSeries, from ,to).Min(m => m.Low); ;
        }
        
        private List<MonthlyQuoteApiModel> Filter(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {
            return monthlyTimeSeries.MonthlyTimeSeries
                    .Where(m => DateTime.Parse(m.Key) >= from && DateTime.Parse(m.Key) <= to)
                    .Select(m => m.Value).ToList();
        }
    }
}
