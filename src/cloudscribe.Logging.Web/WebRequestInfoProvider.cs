using cloudscribe.Logging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

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

        public string GetIpAddress(bool tryUseXForwardHeader = true)
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

        //public string GetIpAddress(bool tryUseXForwardHeader = true)
        //{
        //    string ip = null;

        //    // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

        //    // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
        //    // for 99% of cases however it has been suggested that a better (although tedious)
        //    // approach might be to read each IP from right to left and use the first public IP.
        //    // http://stackoverflow.com/a/43554000/538763
        //    //
        //    if (tryUseXForwardHeader)
        //    {
        //        ip = GetHeaderValueAs<string>("X-Forwarded-For").TrimEnd(',')
        //        .Split(',')
        //        .AsEnumerable<string>()
        //        .Select(s => s.Trim())
        //        .ToList().FirstOrDefault();
        //    }

        //    // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
        //    if (string.IsNullOrWhiteSpace(ip) && _contextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
        //    {
        //        ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        //    }

        //    if (string.IsNullOrWhiteSpace(ip))
        //    {
        //        ip = GetHeaderValueAs<string>("REMOTE_ADDR");
        //    }

        //    return ip;
        //}

        //public T GetHeaderValueAs<T>(string headerName)
        //{
        //    StringValues values;

        //    if (_contextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
        //    {
        //        string rawValues = values.ToString();   // writes out as Csv when there are multiple.

        //        if (!string.IsNullOrWhiteSpace(rawValues))
        //        {
        //            return (T)Convert.ChangeType(values.ToString(), typeof(T));
        //        }
        //    }
        //    return default(T);
        //}


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
