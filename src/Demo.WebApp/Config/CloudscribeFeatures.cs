using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            var connectionString = config.GetConnectionString("EntityFrameworkConnection");

            services.AddCloudscribeCoreEFStorageMSSQL(connectionString);
            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);
            

            return services;
        }

        public static IServiceCollection SetupCloudscribeFeatures(
            this IServiceCollection services,
            IConfiguration config
            )
        {

            services.AddCloudscribeLogging();


            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            services.AddCloudscribeCoreMvc(config);

            return services;
        }

    }
}
