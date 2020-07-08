using Discord.Commands;
using MizuiroBot.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class SharedModule : ModuleBase<SocketCommandContext>
    {
        [Command("setcommand")]
        [Summary("Creates or modifies a custom command with a specified response.")]
        public async Task SetSharedCommand([Summary("The command to be entered.")] string command, [Remainder] [Summary("The response string for said command.")] string response)
        {
            SharedBotInfo shared = SharedBotInfo.GetSharedInfo(Context.Guild.Id);
            if (shared.CustomCommands.Exists(x => x.Key == command)) {
                CustomCommandInfo customCommand = shared.CustomCommands.Find(x => x.Key == command);
                customCommand.Value = response;
                await Context.Channel.SendMessageAsync($"The repsonse given for the command `{Program.Config.CommandPrefix}{command}` has been updated successfully!");
            }
            else
            {
                CustomCommandInfo customCommand = new CustomCommandInfo() {
                    Key = command,
                    Value = response
                };
                shared.CustomCommands.Add(customCommand);
                await Context.Channel.SendMessageAsync($"Booyah! Your shiny new `{Program.Config.CommandPrefix}{command}` command is ready!");
            }
            shared.SaveSharedInfo();
        }
    }
}
