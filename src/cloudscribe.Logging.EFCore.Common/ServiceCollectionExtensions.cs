using cloudscribe.Logging.Common.Models;
using cloudscribe.Logging.EFCore.Common;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.Logging.EFCore
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddCloudscribeLoggingEFCommon(this IServiceCollection services)
        {
            services.AddTransient<IAddLogItem, LogCommand>();

            return services;
        }
    }
}
