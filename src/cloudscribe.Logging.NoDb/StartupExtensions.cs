// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-25
// Last Modified:			2019-06-28
// 


using cloudscribe.Logging.NoDb;
using cloudscribe.Logging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;
using cloudscribe.Logging;
using cloudscribe.Versioning;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {

        public static IServiceCollection AddCloudscribeLoggingNoDbStorage(
            this IServiceCollection services, 
            IConfiguration configuration,
            bool useSingletons = false
            )
        {
            services.Configure<NoDbLogOptions>(configuration.GetSection("NoDbLogOptions"));
            services.AddTransient<IAddLogItem, LogCommand>();
            services.AddScoped<IVersionProvider, VersionProvider>();
            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();

            if (useSingletons)
            {
                services.TryAddSingleton<IStoragePathResolver<LogItem>, LogItemStoragePathResolver>();
                services.AddNoDbSingleton<LogItem>();
                services.AddSingleton<ILogRepository, LogRepository>();
            }
            else
            {
                services.TryAddScoped<IStoragePathResolver<LogItem>, LogItemStoragePathResolver>();
                services.AddNoDb<LogItem>();
                services.AddScoped<ILogRepository, LogRepository>();
            }
            
           

            return services;
        }


    }
}
