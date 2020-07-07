using MizuiroBot.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MizuiroBot
{
    public class Config
    {
        public string DiscordBotToken = "";
        public string TwitchBotUsername = "";
        public string TwitchOAuth2 = "";
        public char CommandPrefix = '!';
        //public string[] TwitchChannels = new string[] { "" };
        public const string ConfigDir = @".\Config.json";

        public static Config LoadConfig()
        {
            CVTS.WriteLineInfo("Attempting to load program configuration file...");
            if (File.Exists(ConfigDir))
            {
                CVTS.WriteLineInfo($"{Path.GetFileName(ConfigDir)} found, will now attempt to load it.");
                try
                {
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigDir));
                    CVTS.WriteLineOk($"{Path.GetFileName(ConfigDir)} loaded successfully!");
                    return config;
                }
                catch
                {
                    CVTS.WriteLineError($"Failed to load {Path.GetFileName(ConfigDir)}! Please check for any errors and restart the application.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else
            {
                CVTS.WriteLineError($"No config file found! A blank template under the name '{Path.GetFileName(ConfigDir)}' will be made in the application directory.");
                File.WriteAllText(ConfigDir, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
                Console.ReadKey();
                Environment.Exit(0);
            }
            return null;
        }
    }
}
