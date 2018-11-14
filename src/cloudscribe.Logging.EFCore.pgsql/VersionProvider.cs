using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Logging.EFCore.pgsql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(LoggingDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("1ed49467-fbd7-43f4-b8ae-5e78294ed071"); } }

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
