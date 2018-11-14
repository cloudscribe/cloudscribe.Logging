using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Logging.NoDb
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(NoDbLogOptions).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("ce0a14ad-c28e-4ab6-a6da-f37df5751b34"); } }

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
