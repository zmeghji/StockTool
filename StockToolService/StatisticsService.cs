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
    }
    public class StatisticsService : IStatisticsService
    {

        public double Avg(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {
            return Filter(monthlyTimeSeries, from, to).Average(m => m.Close);
        }

        public double Min(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {
            return Filter(monthlyTimeSeries, from ,to).Min(m => m.Low); ;
        }
        
        private List<MonthQuoteApiModel> Filter(MonthlyTimeSeriesApiModel monthlyTimeSeries, DateTime from, DateTime to)
        {
            return monthlyTimeSeries.MonthlyTimeSeries
                    .Where(m => DateTime.Parse(m.Key) >= from && DateTime.Parse(m.Key) <= to)
                    .Select(m => m.Value).ToList();
        }
    }
}
