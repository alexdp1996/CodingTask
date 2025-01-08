using Integrations;
using Integrations.Api;
using Interfaces.Integrations;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Repositories;
using Services;

namespace DI
{
    public static class DIExtensions
    {
        static public void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<ApplicationSettings>();

            services.AddScoped<IOrderBookService, OrderBookService>();
            services.AddScoped<IOrderBookProvider, OrderBookProvider>();

            services.AddDbContext<DatabaseContext>((sp, options) =>
                options.UseNpgsql(settings.ConnectionString));

            services.AddRefitClient<IBitstampApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = settings.BitstampUri;
                c.Timeout = c.Timeout = Timeout.InfiniteTimeSpan;
            });

            services.AddTransient<IOrderBookRepository, OrderBookRepository>();
        }
    }
}
