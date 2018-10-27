// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-26
// Last Modified:			2018-10-27
// 

using cloudscribe.Logging.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting // so it will show up in startup without a using
{
    public static class LoggingEFStartup
    {
        public static async Task InitializeDatabaseAsync(
            IServiceProvider serviceProvider,
            int deletePostsOlderThanDays = 0
            )
        {
            
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ILoggingDbContext>();
                await db.Database.MigrateAsync();
                if(deletePostsOlderThanDays > 0)
                {
                    var cutoffDate = DateTime.UtcNow.AddDays(-deletePostsOlderThanDays);
                    var query = from l in db.LogItems
                                where l.LogDateUtc < cutoffDate
                                select l;

                    db.LogItems.RemoveRange(query);
                    await db.SaveChangesAsync();
                }
                   
            }
        }
    }
}
