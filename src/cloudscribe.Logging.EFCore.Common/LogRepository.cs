// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2019-08-31
// 

using cloudscribe.Logging.Models;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EFCore.Common
{
    public class LogRepository : ILogRepository
    {
        public LogRepository(
            ILoggingDbContextFactory loggingDbContextFactory,
            ITruncateLog truncateLog
            )
        {
            _contextFactory = loggingDbContextFactory;
            _truncator = truncateLog;

        }

        private readonly ILoggingDbContextFactory _contextFactory;
        private readonly ITruncateLog _truncator;


        public async Task<int> GetCount(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var db = _contextFactory.CreateContext())
            {
                return await db.LogItems
                   .Where(l => (logLevel == "" || l.LogLevel == logLevel))
                   .CountAsync<LogItem>(cancellationToken);
            }

               
        }

        
        public async Task<PagedResult<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var db = _contextFactory.CreateContext())
            {
                var query = db.LogItems.OrderBy(x => x.LogDateUtc)
                .AsSingleQuery()
                .Where(l => (logLevel == "" || l.LogLevel == logLevel))
                .Skip(offset)
                .Take(pageSize)
                .Select(p => p);

                var result = new PagedResult<ILogItem>()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                result.Data = await query.AsNoTracking().ToListAsync<ILogItem>(cancellationToken);
                result.TotalItems = await GetCount(logLevel, cancellationToken);
                return result;
            }

                
        }

        public async Task<PagedResult<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var db = _contextFactory.CreateContext())
            {
                var query = db.LogItems.OrderByDescending(x => x.LogDateUtc)
                .AsSingleQuery()
                .Where(l => (logLevel == "" || l.LogLevel == logLevel))
                .Skip(offset)
                .Take(pageSize)
                .Select(p => p);

                var result = new PagedResult<ILogItem>()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                result.Data = await query.AsNoTracking().ToListAsync<ILogItem>(cancellationToken);
                result.TotalItems = await GetCount(logLevel, cancellationToken);
                return result;
            }

            

        }

        public async Task DeleteAll(
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var db = _contextFactory.CreateContext())
            {
                if (string.IsNullOrWhiteSpace(logLevel))
                {
                    //db.LogItems.RemoveAll();
                    await _truncator.TruncateLog();
                }
                else
                {
                    var query = from l in db.LogItems
                                where l.LogLevel == logLevel
                                select l;

                    db.LogItems.RemoveRange(query);
                    int rowsAffected = await db.SaveChangesAsync(cancellationToken);
                }


                
            }

            

           
        }

        public async Task Delete(
            Guid logItemId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var db = _contextFactory.CreateContext())
            {
                var itemToRemove = await db.LogItems.SingleOrDefaultAsync(x => x.Id.Equals(logItemId));
                if (itemToRemove != null)
                {
                    db.LogItems.Remove(itemToRemove);
                    int rowsAffected = await db.SaveChangesAsync(cancellationToken);

                }
            }
            
        }

        public async Task DeleteOlderThan(
            DateTime cutoffDateUtc,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var db = _contextFactory.CreateContext())
            {
                if (string.IsNullOrWhiteSpace(logLevel))
                {
                    var query = from l in db.LogItems
                                where l.LogDateUtc < cutoffDateUtc
                                select l;

                    db.LogItems.RemoveRange(query);
                }
                else
                {
                    var query = from l in db.LogItems
                                where l.LogDateUtc < cutoffDateUtc
                                && (l.LogLevel == logLevel)
                                select l;

                    db.LogItems.RemoveRange(query);
                }

                int rowsAffected = await db.SaveChangesAsync(cancellationToken);
            }
            
                 
        }

        

    }
}
