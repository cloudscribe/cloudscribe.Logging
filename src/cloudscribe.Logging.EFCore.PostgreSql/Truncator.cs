﻿using cloudscribe.Logging.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EFCore.PostgreSql
{
    public class Truncator : ITruncateLog
    {
        public Truncator(ILoggingDbContextFactory loggingDbContextFactory)
        {
            _contextFactory = loggingDbContextFactory;
        }

        private readonly ILoggingDbContextFactory _contextFactory;

        public async Task TruncateLog()
        {
            using (var db = _contextFactory.CreateContext())
            {
                await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE cs_system_log; ");
            }
        }
    }
}
