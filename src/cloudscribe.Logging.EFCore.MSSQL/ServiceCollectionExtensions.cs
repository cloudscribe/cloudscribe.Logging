using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.MSSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingEFStorageMSSQL(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null,
            bool useSql2008Compatibility = false
            )
        {
            //services.AddEntityFrameworkSqlServer()
            //    .AddDbContext<LoggingDbContext>((serviceProvider, options) =>
            //    {
            //        options.UseSqlServer(connectionString);
            //    });

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<LoggingDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            if (maxConnectionRetryCount > 0)
                            {
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                sqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: maxConnectionRetryCount,
                                    maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                    errorNumbersToAdd: transientSqlErrorNumbersToAdd);
                            }

                            if (useSql2008Compatibility)
                            {
                                sqlOptions.UseRowNumberForPaging();
                            }

                        }));

            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddScoped<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
