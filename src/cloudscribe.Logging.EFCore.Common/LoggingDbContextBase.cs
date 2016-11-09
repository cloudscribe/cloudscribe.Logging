// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-10
// Last Modified:			2016-11-09
// 

using cloudscribe.Logging.Web;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Logging.EFCore
{
    public class LoggingDbContextBase : DbContext
    {
        
        public LoggingDbContextBase(DbContextOptions options) : base(options)
        {
            
              
        }

        public DbSet<LogItem> LogItems { get; set; }
        
    }
}
