using cloudscribe.Logging.EF;
using cloudscribe.Logging.Web;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingEFStorage(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFramework()
                        .AddSqlServer()
                        .AddDbContext<LoggingDbContext>(options =>
                        {
                            options.UseSqlServer(connectionString);
                        });

            services.TryAddScoped<ILogModelMapper, SqlServerLogModelMapper>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}
