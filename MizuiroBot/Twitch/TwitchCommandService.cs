using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace MizuiroBot.Twitch
{
    public class TwitchCommandService
    {
        public void AddModules(Assembly asm)
        {
            var commandModules = from t in asm.GetTypes()
                           where t.IsAssignableFrom(typeof(TwitchCommandModule))
                           select t;

        }

        public void ExecuteCommand(ITwitchClient client, ChatCommand command)
        {

        }
    }
}
