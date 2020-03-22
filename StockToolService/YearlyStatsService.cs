using AlphaVantageClient.ApiModels;
using AlphaVantageClient.ApiServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockToolServices
{
    public interface IYearlyStatsService
    {
        Task<(double Min, double Avg)> GetMinAndAvg(string symbol, DateTime endDate, int numberOfYears);
        Task<List<(double Min, double Avg, int NumberOfYears)>> GetMinAndAvg(string symbol, DateTime endDate, List<int> numberOfYearsList);

    }
    public class YearlyStatsService: IYearlyStatsService
    {
        private readonly IMonthlyTimeSeriesApiService _monthlyTimeSeriesApiService;
        private readonly IStatisticsService _statisticsService;

        public YearlyStatsService(
            IMonthlyTimeSeriesApiService monthlyTimeSeriesApiService,
            IStatisticsService statisticsService)
        {
            _monthlyTimeSeriesApiService = monthlyTimeSeriesApiService;
            _statisticsService = statisticsService;
        }

        public async Task<(double Min, double Avg)> GetMinAndAvg(string symbol, DateTime endDate, int numberOfYears)
        {
            var monthlyTimeSeries = await _monthlyTimeSeriesApiService.Get(symbol);
            
            var fiveYearMinimum = _statisticsService.Min(
                monthlyTimeSeries: monthlyTimeSeries,
                from: endDate.AddYears(-1*numberOfYears),
                to: endDate);

            var fiveYearAverage = _statisticsService.Avg(
                monthlyTimeSeries: monthlyTimeSeries,
                from: endDate.AddYears(-1 * numberOfYears),
                to: endDate);
            return (Min: fiveYearMinimum, Avg: fiveYearAverage);
        }
        

        public async Task<List<(double Min, double Avg, int NumberOfYears)>> GetMinAndAvg(string symbol, DateTime endDate, List<int> numberOfYearsList)
        {
            var monthlyTimeSeries = await _monthlyTimeSeriesApiService.Get(symbol);
            var statsList = new List<(double Min, double Avg, int NumberOfYears)>();
            foreach(var numberOfYears in numberOfYearsList)
            {
                var fiveYearMinimum = _statisticsService.Min(
                monthlyTimeSeries: monthlyTimeSeries,
                from: endDate.AddYears(-1 * numberOfYears),
                to: endDate);

                var fiveYearAverage = _statisticsService.Avg(
                    monthlyTimeSeries: monthlyTimeSeries,
                    from: endDate.AddYears(-1 * numberOfYears),
                    to: endDate);

                statsList.Add((Min: fiveYearMinimum, Avg: fiveYearAverage, NumberOfYears: numberOfYears));
            }
            
            return statsList;
        }
    }
}
