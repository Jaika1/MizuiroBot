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
            TwitchClient client = new TwitchClient();
            await client.Connect(credentials.TwitchBotUsername, credentials.TwitchOAuth2);
            await client.JoinChannel("jaika1");
            await Task.Delay(-1);
        }
    }

    public struct TwitchCreds
    {
        public string TwitchBotUsername;
        public string TwitchOAuth2;
    }
}
