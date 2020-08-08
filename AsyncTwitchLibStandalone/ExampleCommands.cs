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

        [TwitchCommand("reply")]
        public async Task ReplyCommand(string[] userInput)
        {
            await Channel.SendChatMessage($"You said: {string.Join(' ', userInput)}");
        }

        [TwitchCommand("replynum")]
        public async Task ReplyNumCommand(int userInput)
        {
            await Channel.SendChatMessage($"You said: {userInput}");
        }
    }
}
