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
        private static List<SharedBotInfo> sharedInfoCollection; // = new List<SharedBotInfo>();
        private const string SharedInfoLocation = @".\sharedBotInfo.json";

        public ulong DiscordGuildId = 0;
        public string TwitchChannelName = "";
        public Dictionary<string, string> CustomCommands = new Dictionary<string, string>(); //Key is the command identifier, value is the response.

        public SharedBotInfo() { }

        public SharedBotInfo(ulong discordGuild) { DiscordGuildId = discordGuild; }

        public SharedBotInfo(string twitchChannel) { TwitchChannelName = twitchChannel; }

        public static void LoadSharedInfo()
        {
            sharedInfoCollection = JsonConvert.DeserializeObject<List<SharedBotInfo>>(File.ReadAllText(SharedInfoLocation));
        }

        public static void SaveSharedInfo()
        {
            File.WriteAllText(SharedInfoLocation, JsonConvert.SerializeObject(sharedInfoCollection));
        }

        public static SharedBotInfo GetSharedInfo(ulong discordGuild)
        {
            // Usage of a lesser known operator, the 'null-coalescing operator'. If the left is null, it will instead use the right hand side.
            // In this example, if shared info doesn't exist for the specified discord guild, it will instead be created.
            return sharedInfoCollection.Find(i => i.DiscordGuildId == discordGuild) ?? new SharedBotInfo(discordGuild);
        }

        public static SharedBotInfo GetSharedInfo(string twitchChannel)
        {
            // Usage of a lesser known operator, the 'null-coalescing operator'. If the left is null, it will instead use the right hand side.
            // In this example, if shared info doesn't exist for the specified discord guild, it will instead be created.
            return sharedInfoCollection.Find(i => i.TwitchChannelName == twitchChannel) ?? new SharedBotInfo(twitchChannel);
        }

        public void SetDiscord(ulong discordGuild)
        {

        }
    }
}
