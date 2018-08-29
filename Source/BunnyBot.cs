using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Http;
using Discord.v6;

namespace BunnyBot
{
    class Program
    {
        public static string BotToken = Environment.GetEnvironmentVariable("BUNNYBOT_TOKEN");
        public static string BunnyBotVersion = "1.0";
        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(BotToken))
            {
                Console.WriteLine("Please set BUNNYBOT_TOKEN with the bot token");
                Environment.Exit(1);
            }

            DiscordInfo info = new DiscordInfo
            {
                Token = BotToken,
                Name = "BunnyBot",
                Url = "https://gitlab.com/tim241/BunnyBot",
                Version = BunnyBotVersion
            };

            using (DiscordBot bot = new DiscordBot(info))
            {
                bot.Connect();
            }
        }
    }
}
