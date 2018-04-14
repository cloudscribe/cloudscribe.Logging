using cloudscribe.Logging;
using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.pgsql;
using cloudscribe.Logging.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingEFStoragePostgreSql(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<LoggingDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);

                });

            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();
            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddScoped<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
