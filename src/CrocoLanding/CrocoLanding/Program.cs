using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace CrocoLanding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = GetNLogLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception on setup phase");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureLogging(logging =>
                        {
                            logging.SetMinimumLevel(LogLevel.Trace);
                        })
                        .UseNLog();
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateOnBuild = true;
                });

        private static NLog.Logger GetNLogLogger()
        {
            return NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        }
    }
}