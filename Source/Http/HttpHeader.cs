using System;

namespace Http
{
    public class HttpHeader
    {
        public string GET { get; set; }
        public string POST { get; set; }
        public string HOST { get; set; }
        public string AUTORIZATION { get; set; }
        public string USER_AGENT { get; set; }
        /// <summary>
        /// Generates a valid HTTP header from given HttpHeader
        /// </summary>
        public static string GetHeader(HttpHeader header)
        {
            string returnHeader = null;

            if (!string.IsNullOrEmpty(header.GET))
                returnHeader += $"GET {header.GET} HTTP/1.1\r\n";

            if (!string.IsNullOrEmpty(header.POST))
                returnHeader += $"POST {header.POST}\r\n";

            if (!string.IsNullOrEmpty(header.HOST))
                returnHeader += $"Host: {header.HOST}\r\n";

            if (!string.IsNullOrEmpty(header.AUTORIZATION))
                returnHeader += $"Authorization: {header.AUTORIZATION}\r\n";

            if (!string.IsNullOrEmpty(header.USER_AGENT))
                returnHeader += $"User-Agent: ${header.USER_AGENT}\r\n";

            if(returnHeader == null)
                throw new ArgumentNullException("header");
            
            return returnHeader + "\r\n";
        }
    }
}