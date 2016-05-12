// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2016-05-12
// 

using cloudscribe.Logging.Web;
using cloudscribe.DbHelpers.SqlCe;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.SqlCe
{
#pragma warning disable 1998

    public class LogRepository : ILogRepository
    {
        public LogRepository(
            SqlCeConnectionStringResolver connectionStringResolver)
        {
            if (connectionStringResolver == null) { throw new ArgumentNullException(nameof(connectionStringResolver)); }
            
            connectionString = connectionStringResolver.Resolve();

            dbSystemLog = new DBSystemLog(connectionString);
        }

        
        private string connectionString;
        private DBSystemLog dbSystemLog;

        public void AddLogItem(
            DateTime logDate,
            string ipAddress,
            string culture,
            string url,
            string shortUrl,
            string thread,
            string logLevel,
            string logger,
            string message)
        {
            dbSystemLog.Create(
                logDate,
                ipAddress,
                culture,
                url,
                shortUrl,
                thread,
                logLevel,
                logger,
                message);
        }

        public async Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbSystemLog.GetCount();
        }

        public async Task<List<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<ILogItem> logItems = new List<ILogItem>();
            using (DbDataReader reader = dbSystemLog.GetPageAscending(pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    LogItem logitem = new LogItem();
                    LoadFromReader(logitem, reader);
                    logItems.Add(logitem);
                }
            }

            return logItems;
        }

        public async Task<List<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<ILogItem> logItems = new List<ILogItem>();
            using (DbDataReader reader = dbSystemLog.GetPageDescending(pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    LogItem logitem = new LogItem();
                    LoadFromReader(logitem, reader);
                    logItems.Add(logitem);
                }
            }

            return logItems;
        }

        public async Task DeleteAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            dbSystemLog.DeleteAll();
        }

        public async Task Delete(Guid logItemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            dbSystemLog.Delete(logItemId);
        }

        public async Task DeleteOlderThan(DateTime cutoffDateUtc, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            dbSystemLog.DeleteOlderThan(cutoffDateUtc);
        }

        public async Task DeleteByLevel(string logLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            dbSystemLog.DeleteByLevel(logLevel);
        }

        private void LoadFromReader(LogItem logItem, DbDataReader reader)
        {
            //logItem.Id = Convert.ToInt32(reader["ID"]);
            logItem.Id = new Guid(reader["Id"].ToString());
            logItem.LogDateUtc = Convert.ToDateTime(reader["LogDate"]);
            logItem.IpAddress = reader["IpAddress"].ToString();
            logItem.Culture = reader["Culture"].ToString();
            logItem.Url = reader["Url"].ToString();
            logItem.ShortUrl = reader["ShortUrl"].ToString();
            logItem.Thread = reader["Thread"].ToString();
            logItem.LogLevel = reader["LogLevel"].ToString();
            logItem.Logger = reader["Logger"].ToString();
            logItem.Message = reader["Message"].ToString();

        }


    }

#pragma warning restore 1998
}
