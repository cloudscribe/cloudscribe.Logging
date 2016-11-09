using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EFCore
{
    public interface ILoggingDbContextFactory
    {
        ILoggingDbContext CreateContext();
    }
}
