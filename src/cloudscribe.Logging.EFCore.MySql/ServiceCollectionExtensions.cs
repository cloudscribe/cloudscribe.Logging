using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.EFCore.MySql;
using Microsoft.EntityFrameworkCore;
//using MySQL.Data.EntityFrameworkCore;
//using MySQL.Data.EntityFrameworkCore.Extensions;

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
                    options.UseMySql(connectionString)
                           .UseInternalServiceProvider(serviceProvider);

                });

            services.AddCloudscribeLoggingEFCommon();
            services.AddScoped<ILoggingDbContext, LoggingDbContext>();
            services.AddScoped<ILoggingDbContextFactory, LoggingDbContextFactory>();

            return services;
        }
    }
}
