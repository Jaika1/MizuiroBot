using System;
using System.Collections.Generic;
using System.Text;

namespace MizuiroBot.Twitch
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TwitchCommand : Attribute
    {
        private string commandName;

        public TwitchCommand(string name)
        {
            commandName = name;
        }
    }
}
