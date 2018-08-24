using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Http;

namespace BunnyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpHeader header = new HttpHeader { 
                GET = "/search?q=HelloWorld",
                HOST = "www.google.com"
            };

           using(HttpClient client = new HttpClient("www.google.com", header))
           {
               client.Connect();
           }
        }
    }
}
