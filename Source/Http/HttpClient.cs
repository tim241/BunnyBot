using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net.Security;

namespace Http
{
    /// <summary>
    /// a HTTP client
    /// </summary>
    public class HttpClient : IDisposable
    {
        private TcpClient client { get; set; }
        private NetworkStream stream { get; set; }
        private SslStream sslStream { get; set; }
        private byte[] sslBuffer = new byte[2048];
        private StreamReader reader { get; set; }
        private StreamWriter writer { get; set; }
        private string serverAddress { get; set; }
        private string baseAddress { get; set; }
        private int serverPort { get; set; }
        private bool isHttps { get; set; }
        private HttpHeader header { get; set; }
        /// <summary>
        ///  Creates a new HTTP client, but will not connect until Connect is called
        /// </summary>
        /// <param name="serverAddress">The URL that the HTTP client should connect to</param>
        /// <param name="httpHeader">First header</param>
        public HttpClient(string address, HttpHeader httpHeader)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            isHttps = Http.Url.IsHttps(address);
            baseAddress = Http.Url.GetBase(address);
            header = httpHeader;

            serverAddress = Dns.GetHostEntry(baseAddress).AddressList[0].ToString();
            serverPort = Http.Url.GetPort(address);
        }
        /// <summary>
        /// Connects to HTTP server
        /// </summary>
        public void Connect()
        {
            client = new TcpClient(serverAddress, serverPort);
            if (isHttps)
            {
                sslStream = new SslStream(client.GetStream());
                sslStream.AuthenticateAsClient(baseAddress);
            }
            else
            {
                stream = client.GetStream();
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());
                writer.Write(HttpHeader.GetHeader(header));
                writer.Flush();
                string data = reader.ReadLine();
                Console.WriteLine(data);
            }
        }
        // Implement IDisposable.
        public void Dispose()
        {
            // Dispose everything
            client.Dispose();
            stream.Dispose();
            reader.Dispose();
            writer.Dispose();
            // and finally..
            GC.SuppressFinalize(this);
        }
    }
}