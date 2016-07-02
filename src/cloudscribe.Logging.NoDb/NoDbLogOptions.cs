using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Logging.NoDb
{
    public class NoDbLogOptions
    {
        public NoDbLogOptions()
        {
        }

        public string ProjectId { get; set; } = "default";
    }
}
