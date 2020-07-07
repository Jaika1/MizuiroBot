using Discord.Commands;
using MizuiroBot.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class TwitchModule : ModuleBase<SocketCommandContext>
    {
        [Command("settwitch")]
        [Summary("Sets the Twitch channel of which is associated to this Discord server. Only 1 server can be linked to a channel at any time.")]
        [RequireOwner()]
        public async Task SetTwitchAsync([Summary("The name of the Twitch channel to link.")] string twitchChannelName)
        {
            SharedBotInfo shared = SharedBotInfo.GetSharedInfo(Context.Guild.Id);
            if (shared.SetTwitch(twitchChannelName))
            {
                if (Program.TwitchBot.JoinChannel(twitchChannelName))
                {
                    await Context.Channel.SendMessageAsync("I've found your channel and sent you a friendly little hello message! Can you see me there?");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("I've set myself to associate this server with that Twitch channel, but as of now, I was unable to connect to it.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Another discord guild is already linked to that Twitch channel!");
            }
        }
    }
}
