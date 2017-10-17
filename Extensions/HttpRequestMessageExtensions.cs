using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace ig.log4net.Extensions
{
    public static class HttpRequestMessageExtensions
    {

        public static string GetContentType(this HttpRequestMessage request)
        {
            string retval = String.Empty;
            if (request.Headers.Contains("Content-Type"))
            {
                retval = request.Headers.GetValues("Content-Type").FirstOrDefault();
            }
            if (String.IsNullOrWhiteSpace(retval)) return String.Empty;
            return $"\t{Environment.NewLine}Content-Type: {retval}";

        }

        public static string GetAccept(this HttpRequestMessage request)
        {
            string retval = String.Empty;
            if (request.Headers.Contains("Accept"))
            {
                retval = request.Headers.GetValues("Accept").FirstOrDefault();
            }
            if (String.IsNullOrWhiteSpace(retval)) return String.Empty;
            return $"\t{Environment.NewLine}Accept: {retval}";

        }

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return String.Empty;
        }

        public static string GetRequestId(this HttpRequestMessage request)
        {
            string retval = null;
            if (request.Headers.Contains("requestId"))
            {
                retval = request.Headers.GetValues("requestId").FirstOrDefault();
            }
            if (String.IsNullOrWhiteSpace(retval)) return String.Empty;
            return $"\t{Environment.NewLine}RequestId: {retval}";
        }

        public static string GetClient(this HttpRequestMessage request)
        {
            var host = request.GetClientHostname();
            var ip = request.GetClientIpAddress();
            var result = String.Empty;
            if (String.IsNullOrWhiteSpace(host + ip)) return String.Empty;
            if (!String.IsNullOrWhiteSpace(host)) result = $"\t{Environment.NewLine}Client: {host}";
            if (!String.IsNullOrWhiteSpace(ip)) result = $"{result} ({ip})";
            return result;
        }

        public static string GetClientHostname(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostName;
            }
            return String.Empty;
        }

        public static string GetClientUserAgent(this HttpRequestMessage request)
        {
            string retval = String.Empty;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                retval = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserAgent;
            }
            if (String.IsNullOrWhiteSpace(retval)) return String.Empty;
            return $"\t{Environment.NewLine}User-Agent: {retval}";
        }

        public static string GetPath(this HttpRequestMessage httpRequest)
        {
            var absPath = httpRequest.RequestUri.AbsolutePath;
            return absPath.Substring(absPath.StartsWith("/api/") ? 5 : 1);
        }

        public static string GetHeaderValue(this HttpRequestMessage httpRequest, string key)
            => httpRequest.Headers.GetValues(key).FirstOrDefault() ?? String.Empty;
    }
}
