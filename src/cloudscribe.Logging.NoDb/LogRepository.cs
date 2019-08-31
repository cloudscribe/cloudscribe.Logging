// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-07-02
// Last Modified:			2019-08-31
// 

using cloudscribe.Logging.Models;
using cloudscribe.Pagination.Models;
using Microsoft.Extensions.Options;
using NoDb;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.NoDb
{
    public class LogRepository : ILogRepository
    {
        public LogRepository(
            IBasicCommands<LogItem> commands,
            IBasicQueries<LogItem> queries,
            IStoragePathResolver<LogItem> pathResolver,
            IStringSerializer<LogItem> serializer,
            IOptions<NoDbLogOptions> optionsAccessor
            )
        {
            this.commands = commands;
            query = queries;
            this.serializer = serializer;
            this.pathResolver = pathResolver;
            options = optionsAccessor.Value;

        }

        private IBasicCommands<LogItem> commands;
        private IBasicQueries<LogItem> query;
        private IStringSerializer<LogItem> serializer;
        private IStoragePathResolver<LogItem> pathResolver;
        private NoDbLogOptions options;
        
       

        public async Task DeleteAll(
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                options.ProjectId,
                string.Empty,
                serializer.ExpectedFileExtension
                ).ConfigureAwait(false);

            if(!string.IsNullOrWhiteSpace(logLevel))
            {
                pathToFile = Path.Combine(pathToFile, logLevel);
                if (!Directory.Exists(pathToFile)) { return; }
                var levelDir = new DirectoryInfo(pathToFile);

                // in spite of the bool true which should recursively delete
                // this throws IOException The directory is not empty but does delete the files
                try
                {
                    levelDir.Delete(true);
                }
                catch(IOException)
                { }
                
                return;
            }

            if (!Directory.Exists(pathToFile)) { return; }
            var typeDir = new DirectoryInfo(pathToFile);

            foreach (FileInfo file in typeDir.GetFiles(
                "*" + serializer.ExpectedFileExtension,
                SearchOption.AllDirectories).OrderBy(f => f.CreationTimeUtc)
                )
            {
                file.Delete();
            }

            foreach(var dir in typeDir.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch(IOException)
                { }
                
            }

        }

        public async Task Delete(
            Guid logItemId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await commands.DeleteAsync(
                options.ProjectId,
                logItemId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteOlderThan(
            DateTime cutoffDateUtc,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                options.ProjectId,
                string.Empty,
                serializer.ExpectedFileExtension
                ).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(logLevel))
            {
                if (!Directory.Exists(pathToFile)) { return; }
                // we are in the root type folder
                // so we need to iterate through each direct child folder
                // which corresponds to loglevels
                // and then beneath each of those we need to look for date folders
                // older than the given date
                var typeDir = new DirectoryInfo(pathToFile);
                foreach (var levelDir in typeDir.GetDirectories())
                {
                    foreach (var dateDir in levelDir.GetDirectories())
                    {
                        if(IsOlderThan(dateDir, cutoffDateUtc))
                        {
                            dateDir.Delete(true);
                        }        
                    }
                }

                return;
            }
            else
            {
                pathToFile = Path.Combine(pathToFile, logLevel);
            }

            if (!Directory.Exists(pathToFile)) { return; }

            var levelFolder = new DirectoryInfo(pathToFile);

            foreach (var dateDir in levelFolder.GetDirectories())
            {
                if (IsOlderThan(dateDir, cutoffDateUtc))
                {
                    dateDir.Delete(true);
                }
            }  
        }

        private bool IsOlderThan(DirectoryInfo dateFolder, DateTime cutoffUtc)
        {   
            var folderDate = DateTime.ParseExact(dateFolder.Name, "yyyyMMdd", CultureInfo.InvariantCulture);
            return folderDate.Date < cutoffUtc.Date;
        }

        public virtual async Task<PagedResult<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var pathToFolder = await pathResolver.ResolvePath(options.ProjectId).ConfigureAwait(false);
            var result = new PagedResult<ILogItem>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            if (!Directory.Exists(pathToFolder)) return result;

            if (!string.IsNullOrWhiteSpace(logLevel))
            {
                pathToFolder = Path.Combine(pathToFolder, logLevel);
                if (!Directory.Exists(pathToFolder)) return result;
            }
            
            int offset = (pageSize * pageNumber) - pageSize;
            int skipped = 0;
            int added = 0;

            var dir = new DirectoryInfo(pathToFolder);
            foreach (FileInfo file in dir.GetFiles(
                "*" + serializer.ExpectedFileExtension,
                SearchOption.AllDirectories).OrderBy(f => f.CreationTimeUtc)
                )
            {
                if (offset > 0)
                {
                    if (skipped < offset)
                    {
                        skipped += 1;
                        result.TotalItems += 1;
                        continue;
                    }
                }

                if (added >= pageSize)
                {
                    result.TotalItems += 1;
                    continue;
                }

                var key = Path.GetFileNameWithoutExtension(file.Name);
                var obj = LoadObject(file.FullName, key);
                result.Data.Add(obj);
                added += 1;
                result.TotalItems += 1;
            }

            return result;

        }

        public virtual async Task<PagedResult<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize,
            string logLevel = "",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath(options.ProjectId).ConfigureAwait(false);

            var result = new PagedResult<ILogItem>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            if (!Directory.Exists(pathToFolder)) return result;

            if (!string.IsNullOrWhiteSpace(logLevel))
            {
                pathToFolder = Path.Combine(pathToFolder, logLevel);
                if (!Directory.Exists(pathToFolder)) return result;
            }

            int offset = (pageSize * pageNumber) - pageSize;
            int skipped = 0;
            int added = 0;
            
            var dir = new DirectoryInfo(pathToFolder);
            foreach (FileInfo file in dir.GetFiles(
                "*" + serializer.ExpectedFileExtension,
                SearchOption.AllDirectories).OrderByDescending(f => f.CreationTimeUtc)
                )
            {
                if (offset > 0)
                {
                    if (skipped < offset)
                    {
                        skipped += 1;
                        result.TotalItems += 1;
                        continue;
                    }
                }

                if (added >= pageSize)
                {
                    result.TotalItems += 1;
                    continue;
                }

                var key = Path.GetFileNameWithoutExtension(file.Name);
                var obj = LoadObject(file.FullName, key);
                result.Data.Add(obj);
                added += 1;
                result.TotalItems += 1;
            }
           
            return result;

        }

        protected LogItem LoadObject(string pathToFile, string key)
        {
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                var payload = reader.ReadToEnd();
                var result = serializer.Deserialize(payload, key);
                return result;
            }
        }

        private bool _disposed;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }


    }
}
