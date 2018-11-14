using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Logging.EFCore.MySql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(LoggingDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("22be1404-fdc2-4b14-ae40-8a392020dce4"); } }

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
