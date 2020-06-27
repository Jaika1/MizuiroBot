using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    [Group("Ultility")]
    public class UtilityModule: ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Displays the full list of commands, split into groups. Specify a command name for a more in-depth summary.")]
        public async Task HelpCommand([Summary("The command to learn more about, optional.")] string helpCommandName = "")
        {
            if (string.IsNullOrWhiteSpace(helpCommandName))
            {
                List<EmbedBuilder> groupEmbeds = new List<EmbedBuilder>();
                foreach(ModuleInfo module in Program.DiscordBot.CommandService.Modules)
                {
                    EmbedBuilder groupEmbed = new EmbedBuilder();
                    groupEmbed.Title = module.Group;
                    groupEmbed.Color = new Color(0, 80, 255);
                    foreach(CommandInfo command in module.Commands)
                    {
                        groupEmbed.AddField(new EmbedFieldBuilder()
                        {
                            Name = command.Name,
                            Value = command.Summary
                        });
                    }
                    groupEmbeds.Add(groupEmbed);
                }

                try
                {
                    await Context.Message.Author.SendMessageAsync("**Mizuiro Bot Help:**");
                }
                catch
                {
                    await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + " You aren't accepting DM's from unfriended users, I'm unable to send you the help list!");
                }

                foreach (EmbedBuilder embed in groupEmbeds)
                {
                    await Context.Message.Author.SendMessageAsync(embed: embed.Build());
                }
            }
        }
    }
}
