// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2016-07-01
// 

using cloudscribe.Logging.Web;
using cloudscribe.Logging.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EF
{
    public class LogRepository : ILogRepository
    {
        public LogRepository(
            DbContextOptions<LoggingDbContext> options
            )
        {

            dbContextOptions = options;
            
        }

        // since most of the time this repo will be invoked for adding to the log 
        // we don't need this dbcontext most of the time
        // we do need it for querying the log but we can just create it lazily if it is needed
        private LoggingDbContext dbc = null;
        private LoggingDbContext dbContext
        {
            get
            {
                if(dbc == null)
                {
                    dbc = new LoggingDbContext(dbContextOptions);
                }
                return dbc;
            }
        }

        private DbContextOptions<LoggingDbContext> dbContextOptions;
        //private IServiceProvider serviceProvider;
        
        public void AddLogItem(ILogItem log)
        {
            // since we are using EF to add to the log we need ot avoid
            // logging EF related things, otherwise every time we log we generate more log events
            // continuously
            // might be better to use the normal mssql ado log repository instead
            // need to decouple logging repos from core repos

            if (log.Logger.Contains("EntityFrameworkCore")) return;

            var logItem = LogItem.FromILogItem(log);
            
            using (var context = new LoggingDbContext(dbContextOptions))
            {
                context.Add(logItem);
                context.SaveChanges();
            }

            // learned by experience for this situation we need to create transient instance of the dbcontext
            // for logging because the dbContext we have passed in is scoped to the request
            // and it causes problems to save changes on the context multiple times during a request
            // since we may log mutliple log items in a given request we need to create the dbcontext as needed
            // we can still use the normal dbContext for querying
            //dbContext.Add(logItem);
            //dbContext.SaveChanges();
           
            //return logItem.Id;
        }

        public async Task<int> GetCount(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.LogItems
                .Where(l => (logLevel == "" || l.LogLevel == logLevel))
                .CountAsync<LogItem>(cancellationToken);
        }

        
        public async Task<PagedQueryResult> GetPageAscending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = dbContext.LogItems.OrderBy(x => x.LogDateUtc)
                .Where(l => (logLevel == "" || l.LogLevel == logLevel))
                .Skip(offset)
                .Take(pageSize)
                .Select(p => p)
                ;

            var result = new PagedQueryResult();

            result.Items = await query.AsNoTracking().ToListAsync<ILogItem>(cancellationToken);
            result.TotalItems = await GetCount(logLevel, cancellationToken);
            return result;
        }

        public async Task<PagedQueryResult> GetPageDescending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = dbContext.LogItems.OrderByDescending(x => x.LogDateUtc)
                .Where(l => (logLevel == "" || l.LogLevel == logLevel))
                .Skip(offset)
                .Take(pageSize)
                .Select(p => p)
                ;

            var result = new PagedQueryResult();

            result.Items = await query.AsNoTracking().ToListAsync<ILogItem>(cancellationToken);
            result.TotalItems = await GetCount(logLevel, cancellationToken);
            return result;

        }

        public async Task DeleteAll(
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.LogItems
                        where (logLevel == "" || l.LogLevel == logLevel)
                        select l;

            dbContext.LogItems.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken);

           
        }

        public async Task Delete(
            Guid logItemId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var itemToRemove = await dbContext.LogItems.SingleOrDefaultAsync(x => x.Id.Equals(logItemId));
            if(itemToRemove != null)
            {
                dbContext.LogItems.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken);
                
            }

            
        }

        public async Task DeleteOlderThan(
            DateTime cutoffDateUtc,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.LogItems
                       where l.LogDateUtc < cutoffDateUtc
                       && (logLevel == "" || l.LogLevel == logLevel)
                        select l;

            dbContext.LogItems.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken);
            
            
        }

        


    }
}
