using MizuiroBot;
using MizuiroBot.Discord;
using MizuiroBot.Shared;
using MizuiroBot.Splatoon;
using MizuiroBot.Tools;
using MizuiroBot.Twitch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace MizuiroBot
{
    class Program
    {
        public static Config Config;
        public static DiscordBot DiscordBot = new DiscordBot();
        public static TwitchBot TwitchBot = new TwitchBot();

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
            //TwitchBot.Init(Config.TwitchBotUsername, Config.TwitchOAuth2);
            // Start both bots
            _ = DiscordBot.StartBot(Config.DiscordBotToken); // Needs discard token '_' since this function is asyncronous.
            //TwitchBot.Start();
            // Sleep the thread indefinitely to stop it from closing.
            Thread.Sleep(-1);
        }
    }
}
