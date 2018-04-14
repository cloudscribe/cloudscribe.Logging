using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Logging.Common.Models
{
    public interface IAddLogItem
    {
        void AddLogItem(ILogItem logItem);
    }
}
