using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Logging.Web
{
    public interface IAddLogItem
    {
        void AddLogItem(ILogItem logItem);
    }
}
