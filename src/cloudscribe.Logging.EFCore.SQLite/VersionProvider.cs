using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Logging.EFCore.SQLite
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(LoggingDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("d7252d05-64b3-457e-801c-35ad5dbf4e95"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
