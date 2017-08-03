using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Logging.EFCore.MSSQL
{
    public class LoggingDbContextDesignTimeFactory : IDesignTimeDbContextFactory<LoggingDbContext>
    {
        public LoggingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LoggingDbContext>();
            builder.UseSqlServer("Server=(local);Database=DATABASENAME;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new LoggingDbContext(builder.Options);
        }
    }
}
