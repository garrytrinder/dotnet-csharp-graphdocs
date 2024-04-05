using System.Net;

namespace GraphDocsConnector
{
    internal class Utils
    {
        public static HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler
            {
                Proxy = GetWebProxy()
            };
            return new HttpClient(handler);
        }

        public static IWebProxy? GetWebProxy()
        {
            var customProxy = Environment.GetEnvironmentVariable("CustomProxy");
            if (string.IsNullOrEmpty(customProxy))
            {
                return null;
            }

            return new WebProxy(customProxy);
        }
    }
}
