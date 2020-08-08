using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncTwitchLib
{
    public abstract class TwitchCommandModule
    {
        public TwitchClient Client;
        public TwitchChannel Channel;
    }

    public class TwitchCommand : Attribute
    {
        public readonly string CommandName;
        
        public TwitchCommand(string name)
        {
            CommandName = name;
        }
    }
}
