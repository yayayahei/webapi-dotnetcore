using Hello.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hello
{
    public static class HelloServiceCollectionExtension
    {
        public static void AddHello(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDB(configuration);
        }

        private static void AddDB(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<HelloDbContext>(builder =>
                builder.UseSqlServer(configuration.GetConnectionString("HelloDB")));
        }
    }
}