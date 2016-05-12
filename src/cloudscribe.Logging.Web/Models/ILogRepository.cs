// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2015-12-27
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web
{
    public interface ILogRepository
    {
        void AddLogItem(
            DateTime logDate,
            string ipAddress,
            string culture,
            string url,
            string shortUrl,
            string thread,
            string logLevel,
            string logger,
            string message);

        Task<int> GetCount(CancellationToken cancellationToken);
        Task<List<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        Task<List<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task DeleteAll(CancellationToken cancellationToken);
        Task Delete(Guid logItemId, CancellationToken cancellationToken);
        Task DeleteOlderThan(DateTime cutoffDateUtc, CancellationToken cancellationToken);
        Task DeleteByLevel(string logLevel, CancellationToken cancellationToken);
    }
}
