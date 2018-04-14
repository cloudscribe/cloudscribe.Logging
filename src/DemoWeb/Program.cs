using cloudscribe.Logging.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DemoWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            
            using (var scope = host.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider; 
                try
                {
                    EnsureDataStorageIsReady(scopedServices);

                }
                catch (Exception ex)
                {
                    var logger = scopedServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            var env = host.Services.GetRequiredService<IHostingEnvironment>();
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            ConfigureLogging(env, loggerFactory, host.Services);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    //logging.AddProvider(new DbLoggerProvider(GetLogFilter(hostingContext.HostingEnvironment), logging.Services.));
                })
                .Build();

        private static void EnsureDataStorageIsReady(IServiceProvider scopedServices)
        {
            CoreEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
            
            SimpleContentEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
            
            LoggingEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
        }

       

        private static void ConfigureLogging(
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
            // a customizable filter for logging
            Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
            {
                LogLevel minimumLevel;
                if (env.IsProduction())
                {
                    minimumLevel = LogLevel.Warning;
                }
                else
                {
                    minimumLevel = LogLevel.Information;
                }

                // add exclusions to remove noise in the logs
                var excludedLoggers = new List<string>
                {
                    "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
                    "Microsoft.AspNetCore.Hosting.Internal.WebHost",
                };

                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (excludedLoggers.Contains(loggerName))
                {
                    return false;
                }

                return true;
            };

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }

        //private static Func<string, LogLevel, bool> GetLogFilter(IHostingEnvironment env)
        //{

        //    Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
        //    {
        //        LogLevel minimumLevel;
        //        if (env.IsProduction())
        //        {
        //            minimumLevel = LogLevel.Warning;
        //        }
        //        else
        //        {
        //            minimumLevel = LogLevel.Information;
        //        }

        //        var excludedLoggers = new List<string>
        //        {
        //            "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
        //            "Microsoft.AspNetCore.Hosting.Internal.WebHost",
        //        };

        //        if (logLevel < minimumLevel)
        //        {
        //            return false;
        //        }

        //        if (excludedLoggers.Contains(loggerName))
        //        {
        //            return false;
        //        }

        //        return true;
        //    };

        //    return logFilter;

        //}

        //private static bool LogFilter(string loggerName, LogLevel logLevel)
        //{
        //    var excludedLoggers = new List<string>
        //    {
        //        "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
        //        "Microsoft.AspNetCore.Hosting.Internal.WebHost",
        //    };

        //    if (logLevel < minimumLevel)
        //    {
        //        return false;
        //    }

        //    if (excludedLoggers.Contains(loggerName))
        //    {
        //        return false;
        //    }

        //    return true;

        //}



    }


}
