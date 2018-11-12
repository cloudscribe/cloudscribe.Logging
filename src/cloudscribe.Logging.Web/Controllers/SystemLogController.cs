﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2015-12-23
//	Last Modified:		    2018-11-11
// 

using cloudscribe.Logging.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using cloudscribe.DateTimeUtils;

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
            string sort = "desc")
        {
            ViewData["Title"] = "System Log";
            ViewData["Heading"] = "System Log";

            int itemsPerPage = logManager.LogPageSize;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new LogListViewModel
            {
                LogLevel = logLevel
            };
            PagedQueryResult result;
            if (sort == "desc")
            {
                result = await logManager.GetLogsDescending(pageNumber, itemsPerPage, logLevel);
            }
            else
            {
                result = await logManager.GetLogsAscending(pageNumber, itemsPerPage, logLevel);
            }

            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            model.LogPage = result.Items;

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = result.TotalItems;

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

    }
}
