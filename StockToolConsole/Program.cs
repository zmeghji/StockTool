using AlphaVantageClient.ApiServices;
using AlphaVantageClient.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StockToolServices;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace StockToolConsole
{
    class Program
    {

        private static IConfiguration Configuration;
        private static IServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {

            ConfigureServices();
            var runCommand = _serviceProvider.GetRequiredService<IRunCommand>();
            await runCommand.Execute();
        }
        private static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        private static void ConfigureServices()
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<AlphaVantageAuthenticationOptions>(Configuration.GetSection("AlphaVantageAuthentication"));
            services.AddHttpClient<IQuoteApiService, QuoteApiService>(
                client => client.BaseAddress = new Uri(Configuration.GetValue<string>("ApiUrl")));
            services.AddHttpClient<IMonthlyTimeSeriesApiService, MonthlyTimeSeriesApiService>(
                client => client.BaseAddress = new Uri(Configuration.GetValue<string>("ApiUrl")));
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IRunCommand, RunCommand>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
