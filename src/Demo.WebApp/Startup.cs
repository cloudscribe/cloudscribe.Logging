using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;


namespace Demo.WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration, 
            IHostingEnvironment env,
            ILogger<Startup> logger
            )
        {
            Configuration = configuration;
            Environment = env;
            _log = logger;

            SslIsAvailable = Configuration.GetValue<bool>("AppSettings:UseSsl");
        }

        private IConfiguration Configuration { get; set; }
        private IHostingEnvironment Environment { get; set; }
        private bool SslIsAvailable { get; set; }
        private ILogger _log;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(Configuration, Environment);
            
            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                options.SetupAuthorizationPolicies();

            });

            //// **** IMPORTANT *****
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupDataStorage(Configuration);
            
            //*** Important ***
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupCloudscribeFeatures(Configuration);

            //*** Important ***
            // This is a custom extension method in Config/Localization.cs
            services.SetupLocalization();

            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(SslIsAvailable);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            //app.UseIISIntegration();
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
            }

            


            app.UseStaticFiles();
            app.UseCloudscribeCommonStaticFiles();

            //app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore(
                    loggerFactory,
                    multiTenantOptions,
                    SslIsAvailable);


            app.UseMvc(routes =>
            {
                var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
                //*** IMPORTANT ***
                // this is in Config/RoutingAndMvc.cs
                // you can change or add routes there
                routes.UseCustomRoutes(useFolders);
            });
   
        }

        
        
    }
}