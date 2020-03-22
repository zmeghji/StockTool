using AlphaVantageClient.ApiServices;
using StockToolServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockToolConsole
{

    public interface IRunCommand
    {
        Task Execute();
    }
    public class RunCommand : IRunCommand
    {
        private readonly IQuoteApiService _quoteApiService;
        private readonly IMonthlyTimeSeriesApiService _monthlyTimeSeriesApiService;
        private readonly IStatisticsService _statisticsService;

        public RunCommand(
            IQuoteApiService quoteApiService, 
            IMonthlyTimeSeriesApiService monthlyTimeSeriesApiService,
            IStatisticsService statisticsService)
        {
            _quoteApiService = quoteApiService;
            _monthlyTimeSeriesApiService = monthlyTimeSeriesApiService;
            _statisticsService = statisticsService;
        }
        public async Task Execute()
        {
            while (true)
            {
                WriteLine("Please Enter a Stock Symbol", ConsoleColor.White);
                var symbol = Console.ReadLine();
                var quote = await _quoteApiService.Get(symbol);
                var monthlyTimeSeries = await _monthlyTimeSeriesApiService.Get(symbol);
                //exclude current month from five year min and average
                var from = DateTime.Now.Date.AddMonths(-1).AddYears(-5);
                var to = DateTime.Now.Date.AddMonths(-1);

                var fiveYearMinimum = _statisticsService.Min(
                    monthlyTimeSeries: monthlyTimeSeries,
                    from: from,
                    to: to);

                var fiveYearAverage = _statisticsService.Avg(
                    monthlyTimeSeries: monthlyTimeSeries,
                    from: from,
                    to: to);

                WriteLine("", ConsoleColor.Green);
                WriteLine($"Symbol: {symbol}", ConsoleColor.Green);
                WriteLine($"Price: {quote.Quote.Price}", ConsoleColor.Green);
                WriteLine($"5 Year Min: {fiveYearMinimum}", ConsoleColor.Green);
                WriteLine($"5 Year Avg: {fiveYearAverage}", ConsoleColor.Green);

                WriteLine("", ConsoleColor.Green);
            }
        }
        private void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
    }
}
