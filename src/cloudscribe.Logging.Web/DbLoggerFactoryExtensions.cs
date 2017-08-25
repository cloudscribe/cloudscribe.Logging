// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2017-08-25
// 

using cloudscribe.Logging.Web;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    public static class DbLoggerFactoryExtensions
    {
        public static ILoggerFactory AddDbLogger(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            LogLevel minimumLogLevel)
        {
            Func<string, LogLevel, bool> logFilter = delegate (string loggerName, LogLevel logLevel)
            {
                if (logLevel < minimumLogLevel) { return false; }
               
                return true;
            };

            factory.AddProvider(new DbLoggerProvider(logFilter, serviceProvider));
            return factory;
        }

        public static ILoggerFactory AddDbLogger(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> logFilter)
        {
            factory.AddProvider(new DbLoggerProvider(logFilter, serviceProvider));
            return factory;
        }

        //public static ILoggerFactory AddDbLogger(
        //    this ILoggerFactory factory,
        //    IServiceProvider serviceProvider,
        //    Func<string, LogLevel, bool> logFilter
        //    //, IAddLogItem logRepository
        //    )
        //{
        //    factory.AddProvider(new DbLoggerProvider(logFilter, serviceProvider));
        //    return factory;
        //}

    }
}
