using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        [Command("teirlist")]
        [Summary("Grabs a random teir list from https://tiermaker.com/")]
        public async Task RandomTeirListCommand()
        {

        }
    }
}
