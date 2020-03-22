using AlphaVantageClient.ApiServices;
using AlphaVantageClient.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace StockToolConsole
{
    class Program
    {
        private static IQuoteApiService _quoteApiService;
        private static IConfiguration Configuration;
        private static IServiceProvider _serviceProvider;
        static async Task Main(string[] args)
        {

            ConfigureServices();
            while (true)
            {
                WriteLine("Please Enter a Stock Symbol", ConsoleColor.White);
                var symbol = Console.ReadLine();
                var quote = await _quoteApiService.Get(symbol);
                WriteLine($"Symbol: {symbol} Price: {quote.Quote.Price}", ConsoleColor.Green);
                WriteLine("", ConsoleColor.Green);
            }
        }
        private static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        private static void ConfigureServices()
        {
            Configuration  = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<AlphaVantageAuthenticationOptions>(Configuration.GetSection("AlphaVantageAuthentication"));
            services.AddHttpClient<IQuoteApiService, QuoteApiService>(
                client => client.BaseAddress = new Uri(Configuration.GetValue<string>("ApiUrl"))
                );

            _serviceProvider = services.BuildServiceProvider();
            _quoteApiService = _serviceProvider.GetRequiredService<IQuoteApiService>();
        }
    }
}
