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
            var commands = asm.GetCustomAttributes<TwitchCommand>();
        }

        public void ExecuteCommand(ITwitchClient client, ChatCommand command)
        {

        }
    }
}
