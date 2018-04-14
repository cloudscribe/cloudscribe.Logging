using cloudscribe.Logging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace cloudscribe.Logging.Web
{
    public class WebRequestInfoProvider : IWebRequestInfoProvider
    {
        public WebRequestInfoProvider(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _contextAccessor = httpContextAccessor;
        }

        private IHttpContextAccessor _contextAccessor;

        public string GetIpAddress()
        {
            if ((_contextAccessor != null) && (_contextAccessor.HttpContext != null))
            {
                var connection = _contextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>();
                if (connection != null)
                {
                    return connection.RemoteIpAddress.ToString();
                }

            }
            return null;
        }

        public string GetRequestUrl()
        {
            if ((_contextAccessor != null) && (_contextAccessor.HttpContext != null))
            {
                return _contextAccessor.HttpContext.Request.Path.ToString();
            }

            return null;
        }
    }
}
