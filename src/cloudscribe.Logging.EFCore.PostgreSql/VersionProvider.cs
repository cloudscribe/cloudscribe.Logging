using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Logging.EFCore.PostgreSql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(LoggingDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("62f9ac45-662e-445f-95cf-8a26eeefb6ad"); } }

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
