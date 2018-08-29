using System;
using Http;

namespace Discord.v6
{
    public class DiscordBot : IDisposable
    {
        private static string discordUrl = "https://discordapp.com";
        private string name { get; set; }
        private string url { get; set; }
        private string version { get; set; }
        private string token { get; set; }
        public DiscordBot(DiscordInfo info)
        {
            name = info.Name;
            url = info.Url;
            version = info.Version;
            token = info.Token;
        }
        public void Connect()
        {
            HttpHeader discordHeader = new HttpHeader
            {
                GET = "/api/v6/users/@me",
                HOST = "discordapp.com",
                USER_AGENT = $"{name} ({url}, {version})",
                AUTORIZATION = $"Bot {token}"
            };
            using (HttpClient client = new HttpClient(discordUrl, discordHeader, true))
            {
                string line;


                while ((line = client.ReadLine()) != null)
                {
                    if (client.HttpCode != 200)
                    {
                        Console.WriteLine("invalid response");
                        Environment.Exit(1);
                    }
                    Console.WriteLine(line);
                }

                // Throw away null line
                client.ReadLine();

            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}