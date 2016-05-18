using cloudscribe.Logging.EF;
using cloudscribe.Logging.Web;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<LoggingDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            services.TryAddScoped<ILogModelMapper, SqlServerLogModelMapper>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}
