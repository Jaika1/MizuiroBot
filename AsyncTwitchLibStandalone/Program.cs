using AsyncTwitchLib;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AsyncTwitchLibStandalone
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TwitchCreds credentials = JsonConvert.DeserializeObject<TwitchCreds>(File.ReadAllText(@"Config.json"));
            TwitchIrcClient client = new TwitchIrcClient();
            client.ChatMessageReceived += Client_ChatMessageReceived;
            await client.Connect(credentials.TwitchBotUsername, credentials.TwitchOAuth2);
            await client.JoinChannel("jaika1");
            await Task.Delay(-1);
        }

        private static async Task Client_ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
        {
            Console.WriteLine($"{e.Channel, -24}{e.User, -24}{e.Content}");
        }
    }

    public struct TwitchCreds
    {
        public string TwitchBotUsername;
        public string TwitchOAuth2;
    }
}
