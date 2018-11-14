// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-25
// Last Modified:			2018-04-19
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

        public static IServiceCollection AddCloudscribeLoggingNoDbStorage(this IServiceCollection services, IConfiguration configuration)
        {

            services.TryAddScoped<IWebRequestInfoProvider, NoopWebRequestInfoProvider>();
            services.Configure<NoDbLogOptions>(configuration.GetSection("NoDbLogOptions"));
            services.TryAddScoped<IStoragePathResolver<LogItem>, LogItemStoragePathResolver>();
            services.AddNoDb<LogItem>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddTransient<IAddLogItem, LogCommand>();
            services.AddScoped<IVersionProvider, VersionProvider>();

            return services;
        }


    }
}
