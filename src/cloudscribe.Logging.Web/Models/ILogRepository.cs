// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2017-08-25
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using cloudscribe.Logging.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web
{
    public interface ILogRepository
    {
        //this should be removed, use IAddLogItem instead
        //void AddLogItem(ILogItem logItem);

        //Task<int> GetCount(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedQueryResult> GetPageAscending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedQueryResult> GetPageDescending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAll(string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));
        Task Delete(Guid logItemId, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteOlderThan(DateTime cutoffDateUtc, string logLevel = "", CancellationToken cancellationToken = default(CancellationToken));
        
    }
}
