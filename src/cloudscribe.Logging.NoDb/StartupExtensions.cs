// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-25
// Last Modified:			2016-07-02
// 


using cloudscribe.Logging.NoDb;
using cloudscribe.Logging.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {

        public static IServiceCollection AddCloudscribeLoggingNoDbStorage(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<NoDbLogOptions>(configuration.GetSection("NoDbLogOptions"));
            services.TryAddScoped<IStoragePathResolver<LogItem>, LogItemStoragePathResolver>();
            services.AddNoDb<LogItem>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }


    }
}
