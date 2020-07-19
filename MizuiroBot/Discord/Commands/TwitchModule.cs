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
        public async Task SetTwitch([Summary("The name of the Twitch channel to link.")] string twitchChannelName)
        {
            SharedBotInfo shared = SharedBotInfo.GetSharedInfo(Context.Guild.Id);
            if (shared.SetTwitch(twitchChannelName))
            {
                if (Program.TwitchBot.JoinChannel(twitchChannelName))
                {
                    await Context.Channel.SendMessageAsync("I've found your channel and sent you a friendly little hello message! Can you see me over in Twitch-land?");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("I've set myself a reminder to associate this server with that Twitch channel, but as of now, I was unable to connect to it!");
                }
                shared.SaveSharedInfo();
            }
            else
            {
                await Context.Channel.SendMessageAsync("Another discord server has already been linked to that Twitch channel!");
            }
        }

        [Command("jointwitch")]
        [Summary("A good command if the bot is being stubborn")]
        [RequireOwner()]
        public async Task JoinTwitch()
        {
            SharedBotInfo shared = SharedBotInfo.GetSharedInfo(Context.Guild.Id);
            if (!string.IsNullOrWhiteSpace(shared.TwitchChannelName))
            {
                if (Program.TwitchBot.IsChannelJoined(shared.TwitchChannelName))
                {
                    Program.TwitchBot.SendToChannel("Boo!", shared.TwitchChannelName);
                    await Context.Channel.SendMessageAsync("No need for me to rejoin, I'm already there! Can you see me over in Twitch-land?");
                }
                else
                {
                    if (Program.TwitchBot.JoinChannel(shared.TwitchChannelName))
                    {
                        await Context.Channel.SendMessageAsync("Figure that, I was able to join your Twitch channel this time! Can you see me over in Twitch-land?");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("I wasn't able to connect to the Twitch channel you linked to this server!");
                    }
                }
            } else
            {
                await Context.Channel.SendMessageAsync($"You haven't told me what Twitch channel this server is associated with yet, dummy! *(pssst! use `{Program.Config.CommandPrefix}settwitch`!)*");
            }
        }

        [Command("parttwitch")]
        [Summary("Removes the Twitch channel of which is currently associated to this Discord server.")]
        [RequireOwner()]
        public async Task PartTwitch()
        {
            SharedBotInfo shared = SharedBotInfo.GetSharedInfo(Context.Guild.Id);
            if (!string.IsNullOrWhiteSpace(shared.TwitchChannelName))
            {
                string tChanName = shared.TwitchChannelName;
                shared.RemoveTwitch();
                await Context.Channel.SendMessageAsync($"Pooft! This server is no longer associated with `{tChanName}`!");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Whoopsie! This server isn't even associated with a Twitch channel in the first place!");
            }
        }
    }
}
