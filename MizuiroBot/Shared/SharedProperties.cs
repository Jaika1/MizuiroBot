using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MizuiroBot.Shared
{
    public static class SharedBotInfo
    {
        public static ulong discordGuildId = 0;
        public static string twitchChannelName = "";
        public static Dictionary<string, string> customCommands = new Dictionary<string, string>();
    }
}
