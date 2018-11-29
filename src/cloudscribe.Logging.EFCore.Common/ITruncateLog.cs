using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EFCore.Common
{
    public interface ITruncateLog
    {
        Task TruncateLog();
    }
}
