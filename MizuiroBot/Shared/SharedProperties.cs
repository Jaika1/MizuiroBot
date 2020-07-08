using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MizuiroBot.Shared
{
    public class SharedBotInfo
    {
        private static List<SharedBotInfo> sharedInfoCollection = new List<SharedBotInfo>();
        private const string SharedInfoLocation = @".\sharedBotInfo";

        public static List<SharedBotInfo> SharedInfo => sharedInfoCollection;
        public ulong DiscordGuildId = 0;
        public string TwitchChannelName = "";
        public Dictionary<string, string> CustomCommands = new Dictionary<string, string>(); //Key is the command identifier, value is the response.

        public SharedBotInfo() { }

        public SharedBotInfo(ulong discordGuild) { DiscordGuildId = discordGuild; }

        //public SharedBotInfo(string twitchChannel) { TwitchChannelName = twitchChannel; }

        public static void LoadSharedInfo()
        {
            if (!Directory.Exists(SharedInfoLocation))
            {
                Directory.CreateDirectory(SharedInfoLocation);
            }
            else
            {
                foreach (string file in Directory.EnumerateFiles(SharedInfoLocation, "*.json", SearchOption.TopDirectoryOnly))
                {
                    sharedInfoCollection.Add(JsonConvert.DeserializeObject<SharedBotInfo>(File.ReadAllText(file)));
                }
            }
        }

        public void SaveSharedInfo()
        {
            if (!Directory.Exists(SharedInfoLocation))
            {
                Directory.CreateDirectory(SharedInfoLocation);
            }
            File.WriteAllText(@$"{SharedInfoLocation}\{DiscordGuildId}.json", JsonConvert.SerializeObject(sharedInfoCollection));
        }

        public static SharedBotInfo GetSharedInfo(ulong discordGuild)
        {
            SharedBotInfo shared = sharedInfoCollection.Find(i => i.DiscordGuildId == discordGuild);
            if (shared == null)
            {
                shared = new SharedBotInfo(discordGuild);
                sharedInfoCollection.Add(shared);
            }
            return shared;
        }

        public static SharedBotInfo GetSharedInfo(string twitchChannel)
        {
            return sharedInfoCollection.Find(i => i.TwitchChannelName == twitchChannel);
        }

        public bool SetTwitch(string twitchChannel)
        {
            // Return false if another shared info already references this channel. 
            if (sharedInfoCollection.Find(i => i.TwitchChannelName == twitchChannel) != null) return false;

            if (Program.TwitchBot.IsChannelJoined(TwitchChannelName))
            {
                Program.TwitchBot.LeaveChannel(TwitchChannelName);
            }
            TwitchChannelName = twitchChannel;
            return true;
        }

        public void RemoveTwitch()
        {
            TwitchChannelName = "";
        }
    }
}
