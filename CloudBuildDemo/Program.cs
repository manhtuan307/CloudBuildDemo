using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;

namespace CloudBuildDemo
{
    public class Program
    {
        public static void Main(string[] args) {
            try
            {
                var apiHost = CreateHostBuilder(args).Build();
                var env = apiHost.Services.GetRequiredService<IWebHostEnvironment>();

                var environmentName = env.EnvironmentName;

                // Configure nlog to use Google Stackdriver logging from the XML configuration file.
                LogManager.LoadConfiguration("Configuration/nlog.config");

                // Acquire a logger for this class
                var logger = LogManager.GetCurrentClassLogger();
                // Log some information. This log entry will be sent to Google Stackdriver Logging.
                logger.Info($"[START] Demo! Hello everybody. Environment Name is: {environmentName}");

                apiHost.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("[END] Demo");

                // Flush buffered log entries before program exit; then shutdown the logger before program exit.
                LogManager.Flush(TimeSpan.FromSeconds(15));
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            //var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            //var url = String.Concat("http://0.0.0.0:", port);

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    //webBuilder.UseStartup<Startup>().UseUrls(url);
                    webBuilder.UseStartup<Startup>();
                });
        }

    }
}
