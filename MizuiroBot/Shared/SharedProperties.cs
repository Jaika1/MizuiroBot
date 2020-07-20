using Discord;
using MizuiroBot.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using Color = Discord.Color;

namespace MizuiroBot.Shared
{
    public class SharedBotInfo
    {
        private static List<SharedBotInfo> sharedInfoCollection = new List<SharedBotInfo>();
        private const string SharedInfoLocation = @".\sharedBotInfo";

        public static List<SharedBotInfo> SharedInfo => sharedInfoCollection;
        public ulong DiscordGuildId = 0;
        public string TwitchChannelName = "";
        public List<CustomCommandInfo> CustomCommands = new List<CustomCommandInfo>(); //Key is the command identifier, value is the response.

        public SharedBotInfo() { }

        public SharedBotInfo(ulong discordGuild) { DiscordGuildId = discordGuild; }

        public static void LoadSharedInfo()
        {
            CVTS.WriteLineShared($"Loading shared info from '{SharedInfoLocation}'...");
            if (!Directory.Exists(SharedInfoLocation))
            {
                CVTS.WriteLineShared($"Shared info directory not found! directory will now be created...");
                Directory.CreateDirectory(SharedInfoLocation);
            }
            else
            {
                foreach (string file in Directory.EnumerateFiles(SharedInfoLocation, "*.json", SearchOption.TopDirectoryOnly))
                {
                    sharedInfoCollection.Add(JsonConvert.DeserializeObject<SharedBotInfo>(File.ReadAllText(file)));
                }
            }
            CVTS.WriteLineOk($"Successfully loaded shared info for {sharedInfoCollection.Count} servers!");
        }

        public void SaveSharedInfo()
        {
            CVTS.WriteLineShared($"Attempting to save shared info for discord guild ID {DiscordGuildId}...");
            if (!Directory.Exists(SharedInfoLocation))
            {
                CVTS.WriteLineShared($"Shared info directory not found! directory will now be created...");
                Directory.CreateDirectory(SharedInfoLocation);
            }
            try
            {
                File.WriteAllText(@$"{SharedInfoLocation}\{DiscordGuildId}.json", JsonConvert.SerializeObject(this));
                CVTS.WriteLineOk($"Saved shared info successfully.");
            }
            catch (Exception e)
            {
                CVTS.WriteLineError($"Failed to save shared info! {e.Message}");
            }
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
            Program.TwitchBot.LeaveChannel(TwitchChannelName);
            TwitchChannelName = "";
        }
    }

    public class SharedUserInfo
    {
        private static List<SharedUserInfo> sharedUserCollection = new List<SharedUserInfo>();
        public static List<SharedUserInfo> SharedUsers => sharedUserCollection;
        private const string SharedUserLocation = @".\sharedUserInfo";

        public ulong DiscordUserId = 0ul;
        public string TwitchChannelName = "";
        public string SwitchFc = "";
        public uint ProfileColor = 0x979C9Fu; // Light Grey

        public SharedUserInfo() { }

        public SharedUserInfo(ulong discordId) { DiscordUserId = discordId; }

        public static void LoadUserInfo()
        {
            CVTS.WriteLineShared($"Loading shared user info from '{SharedUserLocation}'...");
            if (!Directory.Exists(SharedUserLocation))
            {
                CVTS.WriteLineShared($"Shared user info directory not found! directory will now be created...");
                Directory.CreateDirectory(SharedUserLocation);
            }
            else
            {
                foreach (string file in Directory.EnumerateFiles(SharedUserLocation, "*.json", SearchOption.TopDirectoryOnly))
                {
                    sharedUserCollection.Add(JsonConvert.DeserializeObject<SharedUserInfo>(File.ReadAllText(file)));
                }
            }
            CVTS.WriteLineOk($"Successfully loaded shared user info for {sharedUserCollection.Count} users!");
        }

        public void SaveUserInfo()
        {
            CVTS.WriteLineShared($"Attempting to save shared info for user with discord id {DiscordUserId}...");
            if (!Directory.Exists(SharedUserLocation))
            {
                CVTS.WriteLineShared($"Shared user info directory not found! directory will now be created...");
                Directory.CreateDirectory(SharedUserLocation);
            }
            try
            {
                File.WriteAllText(@$"{SharedUserLocation}\{DiscordUserId}.json", JsonConvert.SerializeObject(this));
                CVTS.WriteLineOk($"Saved user info successfully.");
            }
            catch (Exception e)
            {
                CVTS.WriteLineError($"Failed to save a users info! {e.Message}");
            }
        }

        public static SharedUserInfo GetUserInfo(ulong discordId)
        {
            SharedUserInfo shared = sharedUserCollection.Find(i => i.DiscordUserId == discordId);
            if (shared == null)
            {
                shared = new SharedUserInfo(discordId);
                sharedUserCollection.Add(shared);
            }
            return shared;
        }

        public static SharedUserInfo GetUserInfo(string twitchChannel)
        {
            return sharedUserCollection.Find(i => i.TwitchChannelName == twitchChannel);
        }

        public bool SetTwitch(string twitchChannel)
        {
            // Return false if another shared info already references this channel. 
            if (sharedUserCollection.Find(i => i.TwitchChannelName == twitchChannel) != null) return false;

            TwitchChannelName = twitchChannel;
            SaveUserInfo();
            return true;
        }

        public void RemoveTwitch()
        {
            TwitchChannelName = "";
        }

        public bool SetColor(string hex)
        {
            System.Drawing.Color htmlColor;
            try
            {
                htmlColor = ColorTranslator.FromHtml(hex);
            } catch
            {
                return false;
            }
            ProfileColor = new Color(htmlColor.R, htmlColor.G, htmlColor.B).RawValue;
            SaveUserInfo();
            return true;
        }

        public Color GetColor()
        {
            return new Color(ProfileColor);
        }

        public bool SetSwitchFc(string code)
        {
            code = code.Replace("SW-", null);
            string[] split = code.Split('-');

            if (split.Length != 3) return false; // Friend codes contain 3 4-digit long sections

            foreach (string s in split)
            {
                int o;
                if (!int.TryParse(s, out o)) return false; // Friend codes do not make use of letters
            }

            SwitchFc = $"SW-{code}";
            SaveUserInfo();
            return true;
        }

        public void RemoveSwitchFc()
        {
            SwitchFc = "";
            SaveUserInfo();
        }
    }

    public class CustomCommandInfo
    {
        public string Key;
        public string Value;
    }
}
