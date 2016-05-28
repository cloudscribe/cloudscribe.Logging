using System;
using System.Collections.Generic;
using cloudscribe.Logging.EF;
using cloudscribe.Logging.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
            IServiceProvider applicationServiceProvider,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()))
                .UseMemoryCache(applicationServiceProvider.GetService<IMemoryCache>())
                .UseLoggerFactory(applicationServiceProvider.GetService<ILoggerFactory>());

            optionsAction?.Invoke(applicationServiceProvider, builder);

            return builder.Options;
        }

        public static IServiceCollection AddCloudscribeLoggingEFStorage(
            this IServiceCollection services,
            string connectionString
            )
        {
            
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<LoggingDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(connectionString)
                           .UseInternalServiceProvider(serviceProvider);

                    //var injectOptions = new DbContextOptions<LoggingDbContext>();
                    //var builder = new DbContextOptionsBuilder(injectOptions);
                    //builder.UseSqlServer(connectionString)
                    //               .UseInternalServiceProvider(serviceProvider);

                    //services.AddSingleton<DbContextOptions<LoggingDbContext>>(injectOptions);

                });

            //if(b != null)
            //{
            //    var injectOptions = b.Options as DbContextOptions<LoggingDbContext>;
            //    if(injectOptions != null)
            //    {
            //        services.AddSingleton<DbContextOptions<LoggingDbContext>>(injectOptions);
            //    }

            //}

            





            //services.AddDbContext<LoggingDbContext>(options =>
            //    {
            //        options.UseSqlServer(connectionString);
            //    });

            services.TryAddScoped<ILogModelMapper, SqlServerLogModelMapper>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}
