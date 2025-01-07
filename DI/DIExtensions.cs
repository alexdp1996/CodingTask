using Integrations;
using Integrations.Api;
using Interfaces.Integrations;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Services;
using System.Text.Json;

namespace DI
{
    public static class DIExtensions
    {
        static public void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderBookService, OrderBookService>();
            services.AddScoped<IOrderBookProvider, OrderBookProvider>();

            services.AddRefitClient<IBitstampApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://www.bitstamp.net/");
                c.Timeout = c.Timeout = Timeout.InfiniteTimeSpan;
            });

            //services.AddTransient<IOrderBookRepository, OrderBookRe>
        }
    }
}
