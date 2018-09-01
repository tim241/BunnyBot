using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net.Security;
using System.Text;

namespace Http
{
    /// <summary>
    /// a HTTP client
    /// </summary>
    public class HttpClient : IDisposable
    {
        private TcpClient client { get; set; }
        private Stream stream { get; set; }
        private SslStream sslStream { get; set; }
        private StreamReader reader { get; set; }
        private StreamWriter writer { get; set; }
        private HttpHeader header { get; set; }
        private string serverAddress { get; set; }
        private string baseAddress { get; set; }
        private int serverPort { get; set; }
        private bool isHttps { get; set; }
        private bool stripHeader { get; set; }
        public int HttpCode { get; set; }
        /// <summary>
        /// Gets a value indicating whether the underlying Socket for a HttpClient is connected to a remote host.
        ///
        /// true if the HttpClient socket was connected to a remote resource as of the most recent operation; otherwise, false.
        /// </summary>
        public bool Connected => client.Connected;
        /// <summary>
        ///  Disposes this HttpClient instance and requests that the underlying TCP connection be closed.
        /// </summary>
        public void Disconnect() => client.Close();
        /// <summary>
        ///  Creates a new HTTP client, but will not connect until Connect is called
        /// </summary>
        /// <param name="serverAddress">The URL that the HTTP client should connect to</param>
        /// <param name="httpHeader">First header</param>
        /// <param name="stripHeader">Strips reponse header, defaults to false</param>
        public HttpClient(string address, HttpHeader httpHeader, bool stripHeader = false)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            if (httpHeader == null)
                throw new ArgumentNullException("httpHeader");

            isHttps = Http.Url.IsHttps(address);
            baseAddress = Http.Url.GetBase(address);
            serverPort = Http.Url.GetPort(address);

            header = httpHeader;

            this.stripHeader = stripHeader;

            serverAddress = Dns.GetHostEntry(baseAddress).AddressList[0].ToString();

            Connect();
        }
        /// <summary>
        /// Sends given HTTP header to server
        /// </summary>
        public void SendHeader(HttpHeader header) => sendData(HttpHeader.GetHeader(header));
        /// <summary>
        /// Sends data to server
        /// </summary>
        private void sendData(string data)
        {
            writer.Write(data);
            writer.Flush();
        }
        private bool strippingHeader = false;
        /// <summary>
        /// Reads a line of characters from the current stream and returns the data as a string.
        /// The next line from the input stream, or null if the end of the input stream is reached.
        /// </summary>
        public string ReadLine()
        {

            string line = reader.ReadLine();

            // start of new header
            if (line.StartsWith("HTTP/1.1 "))
            {
                // reset strippingHeader
                strippingHeader = false;

                int returnCode = 0;

                string[] splitLineArray = line.Split("HTTP/1.1 ");
                string splitLine;

                bool parsingSuccess = false;

                if (splitLineArray.Length > 1)
                {
                    splitLine = splitLineArray[1].Split(' ')[0];
                    parsingSuccess = Int32.TryParse(splitLine, out returnCode);
                }

                if (!parsingSuccess)
                    returnCode = 200;

                HttpCode = returnCode;
            }

            if (!strippingHeader && stripHeader)
            {
                strippingHeader = true;

                while (line.Length != 0)
                {
                    line = ReadLine();
                }

                return ReadLine();
            }

            // seems to be a bug...
            // let's hack it!
            if (line == "0" || line == null)
                return null;

            // possible bug artifacts are here
            // let's just strip it
            if (line.Length <= 4 && line.Length != 0)
                return ReadLine();

            return line;
        }
        /// <summary>
        /// Connects to HTTP server
        /// </summary>
        private void Connect()
        {
            client = new TcpClient(serverAddress, serverPort);
            if (isHttps)
            {
                sslStream = new SslStream(client.GetStream());
                sslStream.AuthenticateAsClient(baseAddress);
                stream = sslStream;
            }
            else
            {
                stream = client.GetStream();
            }
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            sendData(Http.HttpHeader.GetHeader(header));
        }
        // Implement IDisposable.
        public void Dispose()
        {
            // Dispose everything
            if (client != null)
                client.Dispose();
            if (stream != null)
                stream.Dispose();
            if (sslStream != null)
                sslStream.Dispose();
            if (reader != null)
                reader.Dispose();
            if (writer != null)
                writer.Dispose();
            // and finally..
            GC.SuppressFinalize(this);
        }
    }
}