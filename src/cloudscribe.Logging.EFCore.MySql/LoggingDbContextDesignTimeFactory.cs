using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Logging.EFCore.MySql
{
    public class LoggingDbContextDesignTimeFactory : IDesignTimeDbContextFactory<LoggingDbContext>
    {
        public LoggingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LoggingDbContext>();
            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");

            return new LoggingDbContext(builder.Options);
        }
    }
}
