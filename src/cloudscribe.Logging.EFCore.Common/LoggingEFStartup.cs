// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-26
// Last Modified:			2016-11-09
// 


using cloudscribe.Logging.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting // so it will show up in startup without a using
{
    public static class LoggingEFStartup
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ILoggingDbContext>();
                
                await db.Database.MigrateAsync();
                
                
            }
        }
    }
}
