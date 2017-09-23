using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;

namespace cloudscribe.Logging.Web.Models
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Logging.Web"; } }

        public Guid ApplicationId { get { return new Guid("1741afde-e2b8-4d05-9c06-3925c475558f"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 1, 0, 0);
                var versionString = typeof(LogManager).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
