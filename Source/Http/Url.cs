
using System;

namespace Http
{
    public class Url
    {
        /// <summary>
        /// Returns base address
        /// </summary>
        public static string GetBase(string address)
        {
            string returnAddress = address;

            if (address == null)
                throw new ArgumentNullException("url");

            if (address.StartsWith("https://") || address.StartsWith("http://"))
                returnAddress = returnAddress.Split("ttps://")[1];

            if (address.Contains("www."))
                returnAddress = returnAddress.Split("www.")[1];

            if (returnAddress.Contains('/'))
                returnAddress = returnAddress.Split('/')[0];

            return returnAddress;
        }
        /// <summary>
        /// Returns true if given address uses https
        /// </summary>
        public static bool IsHttps(string address) => address.StartsWith("https://");
        /// <summary>
        /// Returns GET request value of given address
        /// </summary>
        public static string GetRequest(string address)
        {
            string[] splitAddress = address.Split(GetBase(address));

            return splitAddress[1];
        }
        /// <summary>
        /// Returns port of given address
        /// </summary>
        public static int GetPort(string address)
        {
            if(address.StartsWith("https://"))
                return 443;
            
            return 80;
        }
    }
}