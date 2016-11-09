using cloudscribe.Logging.Web;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.Logging.EFCore
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddCloudscribeLoggingEFCommon(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>();
            return services;
        }
    }
}
