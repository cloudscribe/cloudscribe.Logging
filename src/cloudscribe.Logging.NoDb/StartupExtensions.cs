// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-25
// Last Modified:			2017-08-25
// 


using cloudscribe.Logging.NoDb;
using cloudscribe.Logging.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {

        public static IServiceCollection AddCloudscribeLoggingNoDbStorage(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.Configure<NoDbLogOptions>(configuration.GetSection("NoDbLogOptions"));
            services.TryAddScoped<IStoragePathResolver<LogItem>, LogItemStoragePathResolver>();
            services.AddNoDb<LogItem>();

            services.AddTransient<IAddLogItem, LogCommand>();

            return services;
        }


    }
}
