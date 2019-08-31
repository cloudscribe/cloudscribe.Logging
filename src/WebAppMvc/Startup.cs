using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.TimeZones;

namespace WebAppMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                //options.SetupAuthorizationPolicies();

                Func<AuthorizationHandlerContext, bool> allowAny = (AuthorizationHandlerContext authContext) => true;

                options.AddPolicy(
                    "SystemLogPolicy",
                    authBuilder =>
                    {
                        //authBuilder.RequireRole("ServerAdmins");
                        authBuilder.RequireAssertion(allowAny);
                    });

            });


            services.AddControllersWithViews();



            var storage = Configuration["DataSettings:DbPlatform"];
            var efProvider = Configuration["DataSettings:EFProvider"];

            switch (storage)
            {
                case "NoDb":
                    
                    services.AddCloudscribeLoggingNoDbStorage(Configuration);
                    break;

                case "ef":
                default:

                    switch (efProvider)
                    {
                        case "sqlite":
                            var slConnection = Configuration["DataSettings:SqliteConnectionString"];
                            
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            break;

                        case "pgsql":
                            var pgsConnection = Configuration["DataSettings:PostgreSqlConnectionString"];
                            
                            services.AddCloudscribeLoggingPostgreSqlStorage(pgsConnection);
                            break;

                        case "mysql":
                            var mysqlConnection = Configuration["DataSettings:MySqlConnectionString"];
                            
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            break;

                        case "mssql":
                        default:
                            var mssqlConnectionString = Configuration["DataSettings:MSSQLConnectionString"];
                            
                            services.AddCloudscribeLoggingEFStorageMSSQL(mssqlConnectionString);

                            break;

                    }

                    break;
            }

            services.AddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));
            services.AddScoped<cloudscribe.DateTimeUtils.ITimeZoneIdResolver, cloudscribe.DateTimeUtils.GmtTimeZoneIdResolver>();
            services.AddScoped<cloudscribe.DateTimeUtils.ITimeZoneHelper, cloudscribe.DateTimeUtils.TimeZoneHelper>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


            services.AddCloudscribeLogging();

            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
