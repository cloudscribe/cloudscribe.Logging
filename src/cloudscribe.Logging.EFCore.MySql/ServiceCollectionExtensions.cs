using cloudscribe.Logging;
using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.MySql;
using cloudscribe.Logging.Models;
using cloudscribe.Versioning;
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

                },
                optionsLifetime: ServiceLifetime.Singleton
                );

            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();
            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddSingleton<ILoggingDbContextFactory, LoggingDbContextFactory>();
            services.AddScoped<IVersionProvider, VersionProvider>();

            return services;
        }
    }
}
