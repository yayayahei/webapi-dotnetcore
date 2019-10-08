using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Hello
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run(); 
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
//                .UseUrls("http://localhost:5002")
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext,logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
//                    logging.AddFilter((category, level) =>
//                        category == DbLoggerCategory.Database.Command.Name
//                        && level == LogLevel.Information);
                })
                .UseNLog();
    }
}
