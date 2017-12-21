using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.SQLite;
using Microsoft.EntityFrameworkCore;

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

                });

            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddScoped<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
