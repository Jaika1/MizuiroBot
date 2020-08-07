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
        public async Task Profile([Summary("A mention of the other users profile you'd like to view.")] SocketUser mentionedUser = null)
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
            profileEmbed.AddField(new EmbedFieldBuilder() { Name = "Rainmaker", Value = (string.IsNullOrWhiteSpace(userInfo.SplatoonData.RainmakerRank) ? "N/A" : userInfo.SplatoonData.RainmakerRank) + (userInfo.SplatoonData.BestRainmakerPower == 0 ? "" : $" ({userInfo.SplatoonData.BestRainmakerPower} highest power)") });
            profileEmbed.AddField(new EmbedFieldBuilder() { Name = "Splat Zones", Value = (string.IsNullOrWhiteSpace(userInfo.SplatoonData.SplatZonesRank) ? "N/A" : userInfo.SplatoonData.SplatZonesRank) + (userInfo.SplatoonData.BestSplatZonesPower == 0 ? "" : $" ({userInfo.SplatoonData.BestSplatZonesPower} highest power)") });
            profileEmbed.AddField(new EmbedFieldBuilder() { Name = "Tower Control", Value = (string.IsNullOrWhiteSpace(userInfo.SplatoonData.TowerControlRank) ? "N/A" : userInfo.SplatoonData.TowerControlRank) + (userInfo.SplatoonData.BestTowerControlPower == 0 ? "" : $" ({userInfo.SplatoonData.BestTowerControlPower} highest power)") });
            profileEmbed.AddField(new EmbedFieldBuilder() { Name = "Clam Blitz", Value = (string.IsNullOrWhiteSpace(userInfo.SplatoonData.ClamBlitzRank) ? "N/A" : userInfo.SplatoonData.ClamBlitzRank) + (userInfo.SplatoonData.BestClamBlitzPower == 0 ? "" : $" ({userInfo.SplatoonData.BestClamBlitzPower} highest power)") });
            profileEmbed.AddField(new EmbedFieldBuilder() { Name = "Best League Power", Value = userInfo.SplatoonData.BestLeaguePower == 0 ? "N/A" : userInfo.SplatoonData.BestLeaguePower.ToString() });
            await Context.Channel.SendMessageAsync(embed: profileEmbed.Build());
        }

        [Command("color")]
        [Summary("Sets the color to be used for your profile! (Use hex, e.g #RRGGBB)")]
        public async Task ProfileColor([Summary("A color in HTML/Hexidecimal format. (hint: google 'Color picker')")] string hex)
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

        [Command("setrainmaker")]
        [Summary("Updates your current rank in Rainmaker for your profile, and secondarily your highest X power (if applicable)")]
        public async Task SetRainmaker([Summary("Your current rank in Rainmaker.")] string rank, [Summary("Your highest X power in Rainmaker.")] float? power = null)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetRainmakerRank(rank, power))
            {
                await Context.Channel.SendMessageAsync($"Successfully updated your rank in Rainmaker!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Uh-oh! Seems like you've specified a rank that doesn't exist!");
            }
        }

        [Command("setzones")]
        [Alias("setsplatzones")]
        [Summary("Updates your current rank in Splat Zones for your profile, and secondarily your highest X power (if applicable)")]
        public async Task SetSplatZones([Summary("Your current rank in Splat Zones.")] string rank, [Summary("Your highest X power in Splat Zones.")] float? power = null)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetSplatZonesRank(rank, power))
            {
                await Context.Channel.SendMessageAsync($"Successfully updated your rank in Splat Zones!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Uh-oh! Seems like you've specified a rank that doesn't exist!");
            }
        }

        [Command("settc")]
        [Alias("settower", "settowercontrol")]
        [Summary("Updates your current rank in Tower Control for your profile, and secondarily your highest X power (if applicable)")]
        public async Task SetTowerControl([Summary("Your current rank in Tower Control.")] string rank, [Summary("Your highest X power in Tower Control.")] float? power = null)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetTowerControlRank(rank, power))
            {
                await Context.Channel.SendMessageAsync($"Successfully updated your rank in Tower Control!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Uh-oh! Seems like you've specified a rank that doesn't exist!");
            }
        }

        [Command("setclams")]
        [Alias("setclamblitz", "setclam")]
        [Summary("Updates your current rank in Clam Blitz for your profile, and secondarily your highest X power (if applicable)")]
        public async Task SetClamBlitz([Summary("Your current rank in Clam Blitz.")] string rank, [Summary("Your highest X power in Clam Blitz.")] float? power = null)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetClamBlitzRank(rank, power))
            {
                await Context.Channel.SendMessageAsync($"Successfully updated your rank in Clam Blitz!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Uh-oh! Seems like you've specified a rank that doesn't exist!");
            }
        }

        [Command("setleague")]
        [Summary("Updates your highest power in League Battle!")]
        public async Task SetLeague([Summary("Your highest power in League Battle.")] float power)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            userInfo.SetLeaguePower(power);
            await Context.Channel.SendMessageAsync($"Successfully updated your highest league power!");
        }

        [Command("locale")]
        [Summary("Sets your preffered locale to use where applicable.")]
        public async Task SetLocale([Summary("The name of the locale.")] string locale)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);
            if (userInfo.SetLocale(locale))
            {
                await Context.Channel.SendMessageAsync($"Your locale was updated successfully.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("That locale wasn't found!");
            }
        }
    }
}
