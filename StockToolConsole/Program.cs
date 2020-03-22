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

        
        private static void ConfigureServices()
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<AlphaVantageAuthenticationOptions>(Configuration.GetSection("AlphaVantageAuthentication"));
            services.RegisterHttpClient<IQuoteApiService, QuoteApiService>(Configuration);
            services.RegisterHttpClient<IMonthlyTimeSeriesApiService, MonthlyTimeSeriesApiService>(Configuration);
            services.RegisterHttpClient<IDailyTimeSeriesApiService, DailyTimeSeriesApiService>(Configuration);
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IRunCommand, RunCommand>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
    public static class IServiceCollectionExtensions
    {
        public static void RegisterHttpClient<TInterface, TImplementation>(this IServiceCollection services, IConfiguration configuration)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddHttpClient<TInterface, TImplementation>(
                client => client.BaseAddress = new Uri(configuration.GetValue<string>("ApiUrl")));
        }
    }
}
