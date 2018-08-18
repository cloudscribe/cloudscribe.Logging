// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-08-25
// Last Modified:			2017-08-25
// 

using cloudscribe.Logging.Models;

namespace cloudscribe.Logging.EFCore.Common
{
    public class LogCommand : IAddLogItem
    {
        public LogCommand(
            ILoggingDbContextFactory contextFactory
            )
        {
            _contextFactory = contextFactory;
        }

        private ILoggingDbContextFactory _contextFactory;

        public void AddLogItem(ILogItem log)
        {
            // since we are using EF to add to the log we need to avoid
            // logging EF related things below warning, otherwise every time we log we generate more log events
            // continuously
            // might be better to use the normal mssql ado log repository instead
            // need to decouple logging repos from core repos

            if (log.Logger.Contains("EntityFrameworkCore"))
            {
                if(log.LogLevel != "Error" && log.LogLevel != "Warning" && log.LogLevel != "Critical")
                {
                    return;
                }
                
            }

            var logItem = LogItem.FromILogItem(log);

            using (var context = _contextFactory.CreateContext())
            {
                context.LogItems.Add(logItem);
                context.SaveChanges();
            }

           
        }


    }
}
