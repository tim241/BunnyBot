
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
            string[] splitAddress;

            if (address == null)
                throw new ArgumentNullException("url");

            foreach (string item in new string[] { "https://", "http://" })
            {
                splitAddress = returnAddress.Split(item);
                if (address.StartsWith(item) &&
                    splitAddress.Length > 1)
                {
                    returnAddress = splitAddress[1];
                }
            }

            splitAddress = returnAddress.Split("www.");
            if (address.Contains("www.") &&
                splitAddress.Length > 1)
            {
                returnAddress = returnAddress.Split("www.")[1];
            }

            // Removes all trailing slashes
            if (returnAddress.Contains('/'))
            {
                splitAddress = returnAddress.Split('/');
                for(int i = 0; i < splitAddress.Length; i++)
                {
                    if(!string.IsNullOrEmpty(splitAddress[i]))
                    {
                        returnAddress = splitAddress[i];
                        break;
                    }
                }
            }

            return returnAddress;
        }
        /// <summary>
        /// Returns true if given address uses https
        /// </summary>
        public static bool IsHttps(string address) => address.StartsWith("https://");
        /// <summary>
        /// Returns GET request value of given address
        /// </summary>
        public static string GetRequest(string address) => address.Split(GetBase(address))[1];
        /// <summary>
        /// Returns port of given address
        /// </summary>
        public static int GetPort(string address)
        {
            if (address.StartsWith("https://"))
                return 443;

            return 80;
        }
    }
}