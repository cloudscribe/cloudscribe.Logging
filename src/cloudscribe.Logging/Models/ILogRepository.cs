// Licensed under the Apache License, Version 2.0
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2019-08-31
// 


using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Models
{
    public interface ILogRepository
    {
       
        Task<PagedResult<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<LogItem>> GetExportData(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<ILogItem>> GetPagedSearchResults(int pageNumber, int pageSize, string searchTerm, string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAll(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));
        Task Delete(Guid logItemId, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteOlderThan(DateTime cutoffDateUtc, string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));
        
    }
}
