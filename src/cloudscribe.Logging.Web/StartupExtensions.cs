using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using cloudscribe.Logging.Web;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLogging(this IServiceCollection services)
        {
            services.AddScoped<LogManager>();
            return services;
        }

        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeLogging(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(LogManager).GetTypeInfo().Assembly,
                    "cloudscribe.Logging.Web"
                ));

            return options;
        }
    }
}
