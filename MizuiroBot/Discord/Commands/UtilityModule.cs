using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class UtilityModule: ModuleBase<SocketCommandContext>
    {
        private Embed helpHeaderEmbed = null;

        [Command("help")]
        [Summary("Displays the full list of commands, split into groups. Specify a command name for a more in-depth summary.")]
        public async Task HelpCommand([Summary("The command to learn more about, optional.")] string helpCommandName = "")
        {
            // Build the help header embed if not done already.
            if (helpHeaderEmbed == null)
            {
                helpHeaderEmbed = new EmbedBuilder()
                {
                    Title = "Mizuiro Bot Help List",
                    Description = "Bot created by Jaika★, public repo available @ https://github.com/Jaika1/MizuiroBot",
                    Color = new Color(20, 150, 255),
                    ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl()
                }.Build();
            }

            // If a specific command has not been specified
            if (string.IsNullOrWhiteSpace(helpCommandName))
            {
                // Get all commands from each module as to split them into groups.
                List<EmbedBuilder> groupEmbeds = new List<EmbedBuilder>();
                foreach(ModuleInfo module in Program.DiscordBot.CommandService.Modules)
                {
                    EmbedBuilder groupEmbed = new EmbedBuilder();
                    groupEmbed.Title = Regex.Replace(module.Name, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1");
                    groupEmbed.Color = new Color(20, 150, 255);
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

                // Attempt to DM the user with the commands list header, good way to decipher if they allow DMs or not.
                try
                {
                    await Context.Message.Author.SendMessageAsync(embed: helpHeaderEmbed);
                }
                catch
                {
                    await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + " You aren't accepting DM's from unfriended users, I'm unable to send you the help list!");
                }

                // Send each command group.
                foreach (EmbedBuilder embed in groupEmbeds)
                {
                    await Context.Message.Author.SendMessageAsync(embed: embed.Build());
                }
            }
        }
    }
}
