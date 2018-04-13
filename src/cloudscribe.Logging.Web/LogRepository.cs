// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2017-08-25
// 

using cloudscribe.Logging.Common.Models;
using cloudscribe.Logging.EFCore;
using cloudscribe.Logging.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web
{
    public class LogRepository : ILogRepository
    {
        public LogRepository(
            ILoggingDbContext loggingContext
            
            )
        {
            //this.contextFactory = contextFactory;
            _dbContext = loggingContext;


        }

        private ILoggingDbContext _dbContext;

        //private ILoggingDbContextFactory contextFactory;
        // since most of the time this repo will be invoked for adding to the log 
        // we don't need this dbcontext most of the time
        // we do need it for querying the log but we can just create it lazily if it is needed
        //private ILoggingDbContext dbc = null;
        //private ILoggingDbContext dbContext
        //{
        //    get
        //    {
        //        if(dbc == null)
        //        {
        //            dbc = contextFactory.CreateContext();
        //        }
        //        return dbc;
        //    }
        //}



        //public void AddLogItem(ILogItem log)
        //{
        //    // since we are using EF to add to the log we need to avoid
        //    // logging EF related things, otherwise every time we log we generate more log events
        //    // continuously
        //    // might be better to use the normal mssql ado log repository instead
        //    // need to decouple logging repos from core repos

        //    if (log.Logger.Contains("EntityFrameworkCore")) return;

        //    var logItem = LogItem.FromILogItem(log);

        //    using (var context = contextFactory.CreateContext())
        //    {
        //        context.LogItems.Add(logItem);
        //        context.SaveChanges();
        //    }

        //    // learned by experience for this situation we need to create transient instance of the dbcontext
        //    // for logging because the dbContext we have passed in is scoped to the request
        //    // and it causes problems to save changes on the context multiple times during a request
        //    // since we may log mutliple log items in a given request we need to create the dbcontext as needed
        //    // we can still use the normal dbContext for querying
        //    //dbContext.Add(logItem);
        //    //dbContext.SaveChanges();

        //    //return logItem.Id;
        //}

        public async Task<int> GetCount(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.LogItems
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

            var query = _dbContext.LogItems.OrderBy(x => x.LogDateUtc)
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

            var query = _dbContext.LogItems.OrderByDescending(x => x.LogDateUtc)
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
            
            if(string.IsNullOrWhiteSpace(logLevel))
            {
                _dbContext.LogItems.RemoveAll();
            }
            else
            {
                var query = from l in _dbContext.LogItems
                        where  l.LogLevel == logLevel
                        select l;

                _dbContext.LogItems.RemoveRange(query);
            }

            
            int rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);

           
        }

        public async Task Delete(
            Guid logItemId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var itemToRemove = await _dbContext.LogItems.SingleOrDefaultAsync(x => x.Id.Equals(logItemId));
            if(itemToRemove != null)
            {
                _dbContext.LogItems.Remove(itemToRemove);
                int rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);
                
            }

            
        }

        public async Task DeleteOlderThan(
            DateTime cutoffDateUtc,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(logLevel))
            {
                var query = from l in _dbContext.LogItems
                            where l.LogDateUtc < cutoffDateUtc
                            select l;

                _dbContext.LogItems.RemoveRange(query);
            }
            else
            {
                var query = from l in _dbContext.LogItems
                            where l.LogDateUtc < cutoffDateUtc
                            && (l.LogLevel == logLevel)
                            select l;

                _dbContext.LogItems.RemoveRange(query);
            }
                
            int rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);
                 
        }

        //#region IDisposable Support

        //private void ThrowIfDisposed()
        //{
        //    if (disposedValue)
        //    {
        //        throw new ObjectDisposedException(GetType().Name);
        //    }
        //}

        //private bool disposedValue = false; // To detect redundant calls

        //void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            // dispose managed state (managed objects).
        //            if(_dbContext != null)
        //            {
        //                _dbContext.Dispose();
        //            }
        //        }

        //        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        //        // TODO: set large fields to null.

        //        disposedValue = true;
        //    }
        //}


        //// This code added to correctly implement the disposable pattern.
        //public void Dispose()
        //{
        //    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    Dispose(true);
        //    // TODO: uncomment the following line if the finalizer is overridden above.
        //    // GC.SuppressFinalize(this);
        //}

        //#endregion

    }
}
