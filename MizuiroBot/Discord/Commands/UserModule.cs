using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MizuiroBot.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        [Command("settwitch")]
        [Summary("Links your Twitch channel to your profile (or changes it if you've already set it)!")]
        public async Task SetTwitch([Summary("The name of the Twitch channel to link.")] string twitchUsername)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetTwitch(twitchUsername))
            {
                await Context.Channel.SendMessageAsync("Your Twitch channel has been linked successfully! I'll keep an eye out for you from now on!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Somebody else has already linked that channel to their own discord account!");
            }
        } 

        [Command("removetwitch")]
        [Summary("Unlinks your Twitch channel from your profile.")]
        public async Task RemoveTwitch()
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (!string.IsNullOrWhiteSpace(userInfo.TwitchChannelName))
            {
                userInfo.RemoveTwitch();
                await Context.Channel.SendMessageAsync("Your Twitch channel has been successfully un-linked!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You haven't even linked a Twitch channel yet!");
            }
        }

        [Command("profile")]
        [Summary("Displays your own (or somebody elses if specified) profile!")]
        public async Task Profile(SocketUser mentionedUser = null)
        {
            mentionedUser ??= Context.User;

            if (mentionedUser.IsBot || mentionedUser.IsWebhook) return;

            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(mentionedUser.Id);
            EmbedBuilder profileEmbed = new EmbedBuilder()
            .WithTitle(mentionedUser.Username)
            .WithThumbnailUrl(mentionedUser.GetAvatarUrl())
            .WithColor(userInfo.GetColor());
            profileEmbed.Description += string.IsNullOrWhiteSpace(userInfo.TwitchChannelName) ? $"Twitch: N/A\n" : $"Twitch: https://www.twitch.tv/{userInfo.TwitchChannelName}\n";
            profileEmbed.Description += string.IsNullOrWhiteSpace(userInfo.SwitchFc) ? $"Switch: N/A\n" : $"Switch: {userInfo.SwitchFc}\n";
            await Context.Channel.SendMessageAsync(embed: profileEmbed.Build());
        }

        [Command("color")]
        [Summary("Sets the color to be used for your profile! (Use hex, e.g #RRGGBB)")]
        public async Task ProfileColor(string hex)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetColor(hex))
            {
                await Context.Channel.SendMessageAsync(embed: new EmbedBuilder().WithColor(userInfo.GetColor()).WithDescription("Set the color of your profile to this! How's it looking?").Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("I dunno what that was you just entered in, but it certainly wasn't right... so no changes have been made.");
            }
        }

        [Command("setswitch")]
        [Summary("Embeds your Switch friend code in your profile (or changes it if you've already set it)!")]
        public async Task SetSwitch([Summary("Your NS friend code (accepts codes with or without the 'SW-' prefix)")] string switchFc)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetSwitchFc(switchFc))
            {
                await Context.Channel.SendMessageAsync($"Your profile should now display `{userInfo.SwitchFc}` as your Switch friend code!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Thats not a Switch friend code, silly!");
            }
        }

        [Command("removeswitch")]
        [Summary("Removes your Switch friend code from your profile.")]
        public async Task RemoveSwitch()
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (!string.IsNullOrWhiteSpace(userInfo.SwitchFc))
            {
                userInfo.RemoveSwitchFc();
                await Context.Channel.SendMessageAsync("Your profile will now no longer display your Switch friend code.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("No Switch friend code has even been assigned to your profile!");
            }
        }
    }
}
