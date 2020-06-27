using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MizuiroBot.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord
{
    public class DiscordBot
    {
        private DiscordSocketClient botUser = new DiscordSocketClient();
        private CommandService commandService = new CommandService();
        private char CommandPrefix => Program.Config.CommandPrefix;
        public CommandService CommandService => commandService;

        public void Init()
        {
            CVTS.WriteLineDiscord("Initialising the Discord bot...");
            botUser.MessageReceived += MessageReceived;
            CVTS.WriteLineDiscord("Initialization of the Discord bot has completed!");
        }

        public async Task StartBot(string botToken)
        {
            CVTS.WriteLineDiscord("Building command service...");
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            CVTS.WriteLineDiscord("Attempting to login to discord with the specified bot token...");
            await botUser.LoginAsync(TokenType.Bot, botToken);
            CVTS.WriteLineDiscord("Login succesful, starting bot service...");
            await botUser.StartAsync();
            CVTS.WriteLineOk("Discord bot started successfully!");
#if DEBUG
            await botUser.SetStatusAsync(UserStatus.Idle);
            await botUser.SetGameAsync("Testing Mode");
#endif
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            SocketUserMessage umsg = (SocketUserMessage)msg;
            if (msg == null) return;
            if (msg.Content == "helpTest")
            {
                return;
            }

            int argPos = 0;

            if (!umsg.HasCharPrefix(CommandPrefix, ref argPos) ||
                umsg.Author.IsBot ||
                umsg.Author.IsWebhook ||
                umsg.Channel is SocketDMChannel) return;

            var context = new SocketCommandContext(botUser, umsg);

            IResult commandResult = await commandService.ExecuteAsync(context, argPos, null);
            if (commandResult.Error.HasValue && commandResult.Error.Value != CommandError.UnknownCommand)
            {
                CVTS.WriteLineError(commandResult.ErrorReason);
            }
        }
    }
}
