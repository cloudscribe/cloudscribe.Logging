using cloudscribe.Logging.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EFCore
{
    public interface ILoggingDbContext : IDisposable
    {
        DbSet<LogItem> LogItems { get; set; }
        ChangeTracker ChangeTracker { get; }

        DatabaseFacade Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
