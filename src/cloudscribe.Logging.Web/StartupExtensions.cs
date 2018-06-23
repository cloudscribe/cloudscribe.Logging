using cloudscribe.Logging.Models;
using cloudscribe.Logging.Web;
using cloudscribe.Logging.Web.Models;
using cloudscribe.Web.Common.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLogging(
            this IServiceCollection services,
            IConfiguration config = null
            )
        {
            services.AddScoped<IWebRequestInfoProvider, WebRequestInfoProvider>();
            services.AddScoped<LogManager>();
            services.AddScoped<IVersionProvider, VersionProvider>();

            if(config != null)
            {
                services.Configure<DbLoggerConfig>(config.GetSection("DbLoggerConfig"));
            }
            

            return services;
        }
        
        public static AuthorizationOptions AddCloudscribeLoggingDefaultPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(
                    "SystemLogPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins");
                    });

            return options;
        }

    }
}
