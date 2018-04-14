// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2014-08-19
//	Last Modified:		    2018-04-14
// 

using cloudscribe.Logging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace cloudscribe.Logging
{
    public class DbLogger : ILogger
    {
        public DbLogger(
            string loggerName,
            Func<string, LogLevel, bool> filter,
            IServiceProvider serviceProvider
            )
        {
            _loggerName = loggerName;
            _services = serviceProvider;
            Filter = filter ?? ((category, logLevel) => true);
        }

        
        private IServiceProvider _services;
        private string _loggerName = string.Empty;
        private Func<string, LogLevel, bool> _filter;

        public Func<string, LogLevel, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _filter = value;
            }
        }



        #region ILogger

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter
            )
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (exception != null)
            {
                message += Environment.NewLine + exception;
            }
            

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (var scope = _services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var _logCommand = services.GetService<IAddLogItem>();
                var _webInfoProvider = services.GetService<IWebRequestInfoProvider>();

                var logItem = new LogItem
                {
                    EventId = eventId.Id,
                    Message = message,
                    IpAddress = _webInfoProvider.GetIpAddress(),
                    Culture = CultureInfo.CurrentCulture.Name,
                    Url = _webInfoProvider.GetRequestUrl()
                };

                if (!string.IsNullOrEmpty(logItem.Url))
                {
                    // lets not log the log viewer
                    if (logItem.Url.StartsWith("/SystemLog")) return;
                    if (logItem.Url.StartsWith("/systemlog")) return;
                    logItem.ShortUrl = GetShortUrl(logItem.Url);
                }
                
                logItem.Thread = System.Threading.Thread.CurrentThread.Name;
                logItem.Logger = _loggerName;
                logItem.LogLevel = logLevel.ToString();

                if(state != null)
                {
                    try
                    {
                        logItem.StateJson = JsonConvert.SerializeObject(
                        state,
                        Formatting.None,
                        new JsonSerializerSettings
                        {
                            DefaultValueHandling = DefaultValueHandling.Include

                        }
                        );
                    }
                    catch (Exception)
                    {
                        // don't throw exceptions from logger
                        //bool foo = true; // just a line to set a breakpoint so I can see the error when debugging
                    }
                }
                
                
                try
                {
                    _logCommand.AddLogItem(logItem);
                }
                catch (Exception)
                {
                    //bool foo = true; // just a line to set a breakpoint so I can see the error when debugging
                }
            }

            
        }

        

        public bool IsEnabled(LogLevel logLevel)
        {
            //Trace = 1,
            //Debug = 2,
            //Information = 3,
            //Warning = 4,
            //Error = 5,
            //Critical = 6,

            //return (logLevel >= minimumLevel);
            return Filter(_loggerName, logLevel);
        }

        public IDisposable BeginScopeImpl(object state)
        {
            return new NoopDisposable();
        }

        #endregion



        private string GetShortUrl(string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                return url;
            }

            if (url.Length > 255)
            {
                return url.Substring(0, 254);
            }

            return url;
        }






        public IDisposable BeginScope<TState>(TState state)
        {
            //throw new NotImplementedException();
            return new NoopDisposable();
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }

    }
}
