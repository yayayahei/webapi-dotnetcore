using Hello.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hello
{
    public static class HelloServiceCollectionExtension
    {
        public static void AddHello(this IServiceCollection services,IConfiguration configuration,ILoggerFactory loggerFactory)
        {
            services.AddDB(configuration,loggerFactory);
        }
        private static void AddDB(this IServiceCollection services,IConfiguration configuration,ILoggerFactory loggerFactory)
        {
            services.AddDbContext<HelloDbContext>(builder =>
                {
                    builder.UseLoggerFactory(loggerFactory);
                    builder.UseSqlServer(configuration.GetConnectionString("HelloDB"));
                });

        }
    }
}