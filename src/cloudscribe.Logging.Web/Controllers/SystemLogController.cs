// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2015-12-23
//	Last Modified:		    2019-08-31
// 

using cloudscribe.Logging.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using cloudscribe.DateTimeUtils;
using cloudscribe.Pagination.Models;
using System.Text;

namespace cloudscribe.Logging.Web.Controllers
{
    public class SystemLogController : Controller
    {
        public SystemLogController(
            LogManager logManager,
            ITimeZoneIdResolver timeZoneIdResolver)
        {
            this.logManager = logManager;
            this.timeZoneIdResolver = timeZoneIdResolver;
        }

        private LogManager logManager;
        private ITimeZoneIdResolver timeZoneIdResolver;

        [Authorize(Policy = "SystemLogPolicy")]
        public async Task<IActionResult> Index(
            string logLevel = "",
            int pageNumber = 1,
            int pageSize = -1,
            string sort = "desc",
            string searchTerm = "")
        {
            ViewData["Title"] = "System Log";
            ViewData["Heading"] = "System Log";
            int itemsPerPage = logManager.LogPageSize;
            var model = new LogListViewModel
            {
                LogLevel = logLevel
            };
            PagedResult<ILogItem> result;

            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (searchTerm != null && searchTerm != "")
            {
                result = await logManager.GetPagedSearchResults(pageNumber, itemsPerPage, searchTerm, logLevel);
            }
            else if (sort == "desc")
            {
                result = await logManager.GetLogsDescending(pageNumber, itemsPerPage, logLevel);
            }
            else
            {
                result = await logManager.GetLogsAscending(pageNumber, itemsPerPage, logLevel);
            }

            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();
            model.LogPage = result;

            return View(model);
        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogItemDelete(Guid id)
        {
            await logManager.DeleteLogItem(id);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogDeleteAll(string logLevel = "")
        {
            await logManager.DeleteAllLogItems(logLevel);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogDeleteOlderThan(string logLevel = "", int days = 5)
        {
            var cutoffUtc = DateTime.UtcNow.AddDays(-days);

            await logManager.DeleteOlderThan(cutoffUtc, logLevel);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<FileResult> Export()
        {
            try
            {
                var result = await logManager.GetExportData();
                StringBuilder sb = new StringBuilder();

                sb.Append("ID" + ',');
                sb.Append("LOG DATE UTC" + ',');
                sb.Append("IP ADDRESS" + ',');
                sb.Append("CULTURE" + ',');
                sb.Append("URL" + ',');
                sb.Append("THREAD" + ',');
                sb.Append("LOG LEVEL" + ',');
                sb.Append("LOGGER" + ',');
                sb.Append("MESSAGE" + ',');
                sb.Append("STATE" + ',');
                sb.Append("EVENT ID" + ',');
                sb.Append("\r\n");

                for (int j = 0; j < result.Count; j++)
                {
                    sb.Append(result[j].Id.ToString() + ',');
                    sb.Append(result[j].LogDateUtc.ToString() + ',');
                    sb.Append(result[j].IpAddress.ToString() + ',');
                    sb.Append(result[j].Culture.ToString() + ',');
                    sb.Append(result[j].Url.ToString() + ',');
                    sb.Append(result[j].Thread.ToString() + ',');
                    sb.Append(result[j].LogLevel.ToString() + ',');
                    sb.Append(result[j].Logger.ToString() + ',');
                    sb.Append(result[j].Message.ToString() + ',');
                    sb.Append(result[j].StateJson.ToString() + ',');
                    sb.Append(result[j].EventId.ToString() + ',');
                }
                sb.Append("\r\n");

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "LoggingExport.csv");
            }
            catch (Exception)
            {
                //swallow error, don't throw from logger.
            }
            return null;
        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = -1, string searchTerm = "", string logLevel = "")
        {
            int itemsPerPage = logManager.LogPageSize;
            PagedResult<ILogItem> result;

            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new LogListViewModel
            {
                LogLevel = logLevel,
                SearchTerm = searchTerm
            };

            result = await logManager.GetPagedSearchResults(pageNumber, itemsPerPage, searchTerm, logLevel);
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();
            model.LogPage = result;

            return View("Index", model);
        }
    }
}
