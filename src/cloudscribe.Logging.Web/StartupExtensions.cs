using cloudscribe.Logging.Web;
using cloudscribe.Logging.Web.Models;
using cloudscribe.Web.Common.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLogging(this IServiceCollection services)
        {
            services.AddScoped<LogManager>();
            services.AddScoped<IVersionProvider, VersionProvider>();
            return services;
        }

        [Obsolete("AddEmbeddedViewsForCloudscribeLogging is deprecated, please use AddCloudscribeLoggingBootstrap3Views instead.")]
        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeLogging(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(LogManager).GetTypeInfo().Assembly,
                    "cloudscribe.Logging.Web"
                ));

            return options;
        }

        public static RazorViewEngineOptions AddCloudscribeLoggingBootstrap3Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(LogManager).GetTypeInfo().Assembly,
                    "cloudscribe.Logging.Web"
                ));

            return options;
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
