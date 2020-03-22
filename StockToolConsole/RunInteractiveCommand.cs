using AlphaVantageClient.ApiServices;
using StockToolServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockToolConsole
{

    public interface IRunInteractiveCommand
    {
        Task Execute();
    }
    public class RunInteractiveCommand : IRunInteractiveCommand
    {
        private readonly IQuoteApiService _quoteApiService;
        private readonly IStatisticsService _statisticsService;
        private readonly IDailyTimeSeriesApiService _dailyTimeSeriesApiService;
        private readonly IYearlyStatsService _yearlyStatsService;
        private DateTime firstEventStartDate;
        private DateTime secondEventStartDate;

        public RunInteractiveCommand(
            IQuoteApiService quoteApiService, 
            IStatisticsService statisticsService,
            IDailyTimeSeriesApiService dailyTimeSeriesApiService,
            IYearlyStatsService yearlyStatsService)
        {
            _quoteApiService = quoteApiService;
            _statisticsService = statisticsService;
            _dailyTimeSeriesApiService = dailyTimeSeriesApiService;
            _yearlyStatsService = yearlyStatsService;
        }
        public async Task Execute()
        {
            while (true)
            {
                WriteLine("Please Enter Start Date for Event 1 (MM-dd-yyyy)", ConsoleColor.White);
                firstEventStartDate = DateTime.Parse(Console.ReadLine());

                WriteLine("Please Enter Start Date for Event 2 (MM-dd-yyyy)", ConsoleColor.White);
                secondEventStartDate = DateTime.Parse(Console.ReadLine());

                WriteLine("Please Enter a Stock Symbol", ConsoleColor.White);
                var symbol = Console.ReadLine();

                WriteLine("", ConsoleColor.Green);
                WriteLine($"Symbol: {symbol}", ConsoleColor.Green);
                await PrintPrice(symbol);
                await PrintFiveYearStats(symbol);
                await PrintEventDifferentials(symbol);
                WriteLine("", ConsoleColor.Green);
            }
        }

        private async Task PrintPrice(string symbol)
        {
            var quote = await _quoteApiService.Get(symbol);
            WriteLine($"Price: {quote.Quote.Price}", ConsoleColor.Green);
        }
        private void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
        private async Task PrintEventDifferentials(string symbol)
        {
            var dailyTimeSeries = await _dailyTimeSeriesApiService.Get(symbol);

            var event1Change = _statisticsService.Difference(dailyTimeSeries, firstEventStartDate, secondEventStartDate.AddDays(-1));
            var event2Change = _statisticsService.Difference(dailyTimeSeries, secondEventStartDate, DateTime.Now.Date);

            WriteLine($"Event 1 Change: {event1Change}", ConsoleColor.Green);
            WriteLine($"Event 2 Change: {event2Change}", ConsoleColor.Green);
            WriteLine($"E2/E1 Change ratio: {event2Change/event1Change}", ConsoleColor.Green);

        }
        private async Task PrintFiveYearStats(string symbol)
        {
            //Five year period ends at the first event start date
            var fiveYearStats = await _yearlyStatsService.GetMinAndAvg(symbol, firstEventStartDate, 5);
            WriteLine($"5 Year Min: {fiveYearStats.Min}", ConsoleColor.Green);
            WriteLine($"5 Year Avg: {fiveYearStats.Avg}", ConsoleColor.Green);
        }
    }
}
