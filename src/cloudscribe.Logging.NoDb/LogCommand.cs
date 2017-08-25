// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-08-25
// Last Modified:			2017-08-25
// 

using cloudscribe.Logging.Web;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace cloudscribe.Logging.NoDb
{
    public class LogCommand : IAddLogItem
    {
        public LogCommand(IHostingEnvironment appEnv)
        {
            _env = appEnv;
        }

        private IHostingEnvironment _env;
        private string _baseFolderVPath = "nodb_storage";
        private string _projectsFolderName = "projects";
        private string _projectId = "default";
        
        public void AddLogItem(ILogItem log)
        {
            var logItem = LogItem.FromILogItem(log);
            
            var pathToFile = ResolvePath(
                logItem.Id.ToString(),
                logItem,
                ".json",
                true
                );

            if (File.Exists(pathToFile)) return;

            var serialized = JsonConvert.SerializeObject(
                logItem,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include } );

            using (StreamWriter s = File.CreateText(pathToFile))
            {
                s.Write(serialized);
            }

        }
        
        private string ResolvePath(
            string key,
            LogItem logItem,
            string fileExtension = ".json",
            bool ensureFoldersExist = true
            )
        {
            
            var firstFolderPath = Path.Combine(_env.ContentRootPath,  _baseFolderVPath);

            if (ensureFoldersExist && !Directory.Exists(firstFolderPath))
            {
                Directory.CreateDirectory(firstFolderPath);
            }

            var projectsFolderPath = Path.Combine(firstFolderPath, _projectsFolderName);

            if (ensureFoldersExist && !Directory.Exists(projectsFolderPath))
            {
                Directory.CreateDirectory(projectsFolderPath);
            }

            var projectIdFolderPath = Path.Combine(projectsFolderPath, _projectId);

            if (ensureFoldersExist && !Directory.Exists(projectIdFolderPath))
            {
                Directory.CreateDirectory(projectIdFolderPath);
            }

            var type = typeof(LogItem).Name.ToLowerInvariant();

            var typeFolderPath = Path.Combine(projectIdFolderPath, type.ToLowerInvariant().Trim());

            if (ensureFoldersExist && !Directory.Exists(typeFolderPath))
            {
                Directory.CreateDirectory(typeFolderPath);
            }

            var levelPath = Path.Combine(typeFolderPath, logItem.LogLevel.ToString());

            if (ensureFoldersExist && !Directory.Exists(levelPath))
            {
                Directory.CreateDirectory(levelPath);
            }
;
            var dateFolderName = logItem.LogDateUtc.ToString("yyyyMMdd");

            var datePath = Path.Combine(levelPath, dateFolderName);

            if (ensureFoldersExist && !Directory.Exists(datePath))
            {
                Directory.CreateDirectory(datePath);
            }

            // we don't care if this file exists
            // this method is for calculating where to save 
            return Path.Combine(datePath, key + fileExtension);
        }

    }
}
