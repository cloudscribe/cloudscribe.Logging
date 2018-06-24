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

        public string DevLogLevel { get; set; } = "Debug";
        public string ProductionLogLevel { get; set; } = "Warning";

        public List<string> ExcludedNamesSpaces { get; set; }
        public List<string> BelowWarningExcludedNamesSpaces { get; set; }
    }
}
