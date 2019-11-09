using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {
        public static IEndpointRouteBuilder UseCustomRoutes(this IEndpointRouteBuilder routes, bool useFolders)
        {
            //routes.AddCloudscribeFileManagerRoutes();
            if (useFolders)
            {
                routes.MapControllerRoute(
                    name: "foldererrorhandler",
                    pattern: "{sitefolder}/oops/error/{statusCode?}",
                    defaults: new { controller = "Oops", action = "Error" },
                    constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                );

                routes.MapControllerRoute(
                    name: "folderdefault",
                    pattern: "{sitefolder}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" },
                    constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );
            }
            routes.MapControllerRoute(
                name: "errorhandler",
                pattern: "oops/error/{statusCode?}",
                defaults: new { controller = "Oops", action = "Error" }
                );



            routes.MapControllerRoute(
                name: "def",
                pattern: "{controller}/{action}"
                , defaults: new { controller = "Home", action = "Index" }
                );
            
            return routes;
        }

        public static IServiceCollection SetupMvc(
            this IServiceCollection services,
            bool sslIsAvailable
            )
        {
            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.1
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                //options.KnownNetworks.Clear();
                //options.KnownProxies.Clear();
            });

            services.Configure<MvcOptions>(options =>
            {
                if (sslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    //options.AddCloudscribeCommonEmbeddedViews();
                    //options.AddCloudscribeNavigationBootstrap3Views();
                    //options.AddCloudscribeCoreBootstrap3Views();
                    //options.AddCloudscribeFileManagerBootstrap3Views();
                   // options.AddCloudscribeLoggingBootstrap3Views();

                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                });

            return services;
        }

    }
}
