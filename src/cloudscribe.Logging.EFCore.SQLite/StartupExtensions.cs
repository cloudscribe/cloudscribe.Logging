using cloudscribe.Logging;
using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.SQLite;
using cloudscribe.Logging.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingEFStorageSQLite(
            this IServiceCollection services,
            string connectionString
            )
        {
            services
                .AddDbContext<LoggingDbContext>(options =>
                {
                    options.UseSqlite(connectionString);

                },
                optionsLifetime: ServiceLifetime.Singleton
                );

            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();
            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddSingleton<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
