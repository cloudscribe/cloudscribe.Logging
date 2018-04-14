using cloudscribe.Logging.Models;

namespace cloudscribe.Logging
{
    /// <summary>
    /// this default implementation is for logging outside of web apps
    /// </summary>
    public class NoopWebRequestInfoProvider : IWebRequestInfoProvider
    {
        public string GetIpAddress()
        {
            return null;
        }

        public string GetRequestUrl()
        {
            return null;
        }
    }
}
