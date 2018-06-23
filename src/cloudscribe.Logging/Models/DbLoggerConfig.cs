using System.Collections.Generic;

namespace cloudscribe.Logging.Models
{
    public class DbLoggerConfig
    {
        public DbLoggerConfig()
        {
            ExcludedNamesSpaces = new List<string>();
            BelowWarningExcludedNamesSpaces = new List<string>();
        }

        public List<string> ExcludedNamesSpaces { get; set; }
        public List<string> BelowWarningExcludedNamesSpaces { get; set; }
    }
}
