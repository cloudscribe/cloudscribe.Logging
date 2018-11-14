using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Logging.EFCore.MSSQL
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(LoggingDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("71cc4df3-7d16-4a12-ac7a-49360dbfc2b3"); } }

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
