using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        [Command("settwitch")]
        [Summary("Links your Twitch username to your profile (Or changes it if you've already set it)!")]
        public async Task SetTwitch(string twitchUsername)
        {

        } 
    }
}
