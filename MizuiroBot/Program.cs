using MizuiroBot.Discord;
using MizuiroBot.Shared;
using MizuiroBot.Splatoon;
using MizuiroBot.Tools;
using AsyncTwitchLib;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MizuiroBot
{
    class Program
    {
        public static Config Config;
        public static DiscordBot DiscordBot = new DiscordBot();
        public static TwitchClient TwitchBot = new TwitchClient();

        static void Main(string[] args)
        {
            // Enables Console Virtual Terminal Sequences
            CVTS.TryEnable();
            CVTS.WriteLineInfo($"{Process.GetCurrentProcess().ProcessName} version {Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductVersion}, created by Jaika★.");
            // Download/reload Splatoon 2 data from the internet/local storage
            Data.LoadSplatoon2Data();
            // Load the config and shared info.
            Config = Config.LoadConfig();
            SharedBotInfo.LoadSharedInfo();
            SharedUserInfo.LoadUserInfo();
            // Initialise both bots 
            DiscordBot.Init();
            TwitchBot.ChannelJoined += TwitchBot_ChannelJoined;
            TwitchBot.Connected += TwitchBot_Connected;
            // Start both bots
            _ = DiscordBot.StartBot(Config.DiscordBotToken); // Needs discard token '_' since this function is asyncronous.
            _ = TwitchBot.Connect(Config.TwitchBotUsername, Config.TwitchOAuth2);
            // Sleep the thread indefinitely to stop it from closing.
            Thread.Sleep(-1);
        }

        private static async Task TwitchBot_Connected(object sender)
        {
            CVTS.WriteLineTwitch("Joining all specified channels in shared channel info...");
            foreach (SharedBotInfo shared in SharedBotInfo.SharedInfo)
            {
                if (!string.IsNullOrWhiteSpace(shared.TwitchChannelName))
                {
                    CVTS.WriteLineTwitch($"Joining {shared.TwitchChannelName}...");
                    await TwitchBot.JoinChannel(shared.TwitchChannelName);
                }
                else
                {
                    CVTS.WriteLineTwitch($"Channel not found in shared info.");
                }
            }
            CVTS.WriteLineOk("Twitch bot started successfully!");
        }

        private static async Task TwitchBot_ChannelJoined(object sender, ChannelJoinedEventArgs e)
        {
            //await e.Channel.SendChatMessage("I'm alive, yippe!");
            SharedBotInfo.GetSharedInfo(e.Channel.Channel)?.LinkCustomCommands(e.Channel);
        }
    }
}
