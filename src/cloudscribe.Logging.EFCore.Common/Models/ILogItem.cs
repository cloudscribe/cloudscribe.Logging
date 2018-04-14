// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2016-07-01
// 

using System;

namespace cloudscribe.Logging.Common.Models
{
    public interface ILogItem
    {
        Guid Id { get; } 
        DateTime LogDateUtc { get; set; }
        int EventId { get; set; }
        string IpAddress { get; set; } 
        string Culture { get; set; }
        string Url { get; set; } 
        string ShortUrl { get; set; } 
        string Thread { get; set; } 
        string LogLevel { get; set; }
        string Logger { get; set; }
        string Message { get; set; }
        string StateJson { get; set; }

    }
}
