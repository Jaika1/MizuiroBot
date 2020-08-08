using AsyncTwitchLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTwitchLibStandalone
{
    public class ExampleCommands : TwitchCommandModule
    {
        [TwitchCommand("ping")]
        public async Task PingCommand()
        {
            await Channel.SendChatMessage("Pong!");
        }
    }
}
