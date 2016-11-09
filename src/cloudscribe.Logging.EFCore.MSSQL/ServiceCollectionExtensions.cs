using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeLoggingEFStorageMSSQL(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<LoggingDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(connectionString)
                           .UseInternalServiceProvider(serviceProvider);

                });

            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddScoped<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
