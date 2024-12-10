// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-21
//	Last Modified:		    2019-08-31
// 

using cloudscribe.Logging.Models;
using cloudscribe.Pagination.Models;

namespace cloudscribe.Logging.Web
{
    public class LogListViewModel
    {
        public LogListViewModel()
        {
            LogPage = new PagedResult<ILogItem>();
        }

        public string LogLevel { get; set; } = string.Empty;
        public PagedResult<ILogItem> LogPage { get; set; }
        
        public string TimeZoneId { get; set; } = "America/New_York";
        public string SearchTerm { get; set; }
    }
}
