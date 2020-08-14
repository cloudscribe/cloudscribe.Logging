using cloudscribe.Logging;
using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.Common;
using cloudscribe.Logging.EFCore.PostgreSql;
using cloudscribe.Logging.Models;
using cloudscribe.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingPostgreSqlStorage(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<string> transientErrorCodesToAdd = null
            )
        {

            // AddEntityFrameworkNpgsql call should be deprecated:
            // https://www.npgsql.org/efcore/api/Microsoft.Extensions.DependencyInjection.NpgsqlServiceCollectionExtensions.html

            services // .AddEntityFrameworkNpgsql()
                .AddDbContext<LoggingDbContext>(options =>
                    options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorCodesToAdd: transientErrorCodesToAdd);
                        }


                    }),
                    optionsLifetime: ServiceLifetime.Singleton
                    );



            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();
            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddSingleton<ILoggingDbContextFactory, LoggingDbContextFactory>();
            services.AddScoped<IVersionProvider, VersionProvider>();
            services.AddScoped<ITruncateLog, Truncator>();

            return services;
        }
    }
}
