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
            var storage = config["DataSettings:DbPlatform"];
            var efProvider = config["DataSettings:EFProvider"];

            switch (storage)
            {
                case "NoDb":
                    services.AddCloudscribeCoreNoDbStorage();
                    services.AddCloudscribeLoggingNoDbStorage(config);
                    break;

                case "ef":
                default:

                    switch (efProvider)
                    {
                        case "sqlite":
                            var slConnection = config["DataSettings:SqliteConnectionString"];
                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            break;

                        case "pgsql":
                            var pgsConnection = config["DataSettings:PostgreSqlConnectionString"];
                            services.AddCloudscribeCorePostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeLoggingPostgreSqlStorage(pgsConnection);
                            break;

                        case "mysql":
                            var mysqlConnection = config["DataSettings:MySqlConnectionString"];
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            break;

                        case "mssql":
                        default:
                            var mssqlConnectionString = config["DataSettings:MSSQLConnectionString"];
                            services.AddCloudscribeCoreEFStorageMSSQL(mssqlConnectionString);
                            services.AddCloudscribeLoggingEFStorageMSSQL(mssqlConnectionString);

                            break;

                    }

                    break;
            }

            
            

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
