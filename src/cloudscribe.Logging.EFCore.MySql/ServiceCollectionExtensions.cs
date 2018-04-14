using cloudscribe.Logging;
using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.MySql;
using cloudscribe.Logging.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingEFStorageMySQL(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFrameworkMySql()
                .AddDbContext<LoggingDbContext>((serviceProvider, options) =>
                {
                    options.UseMySql(connectionString);

                });

            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();
            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddScoped<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
