using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Logging.EFCore.SQLite
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<LoggingDbContext>
    {
        public LoggingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LoggingDbContext>();
            builder.UseSqlite("Data Source=cloudscribe.db");
            return new LoggingDbContext(builder.Options);
        }
    }
}
