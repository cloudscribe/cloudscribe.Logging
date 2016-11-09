using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Logging.EFCore.MSSQL
{
    public class LoggingDbContextFactory : ILoggingDbContextFactory
    {
        public LoggingDbContextFactory(DbContextOptions<LoggingDbContext> options)
        {
            dbContextOptions = options;
        }

        private DbContextOptions<LoggingDbContext> dbContextOptions;

        public ILoggingDbContext CreateContext()
        {
            return new LoggingDbContext(dbContextOptions);
        }
    }
}
