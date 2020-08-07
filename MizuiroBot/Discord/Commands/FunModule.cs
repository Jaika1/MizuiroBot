using Discord;
using Discord.Commands;
using MizuiroBot.Shared;
using MizuiroBot.Splatoon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Discord.Commands
{
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        private Random random = new Random();

        [Command("randomweapon")]
        [Alias("randwep")]
        [Summary("Picks out a random main weapon for you to use in Splatoon 2 for when you can't decide yourself (or are looking to have a bit of random fun)!")]
        public async Task RandomWeaponCommand([Summary("Your current level, good for shortening the potential selection for those who are not yet at level 30.")] int level = 30)
        {
            SharedUserInfo userInfo = SharedUserInfo.GetUserInfo(Context.User.Id);

            IEnumerable<MainWeaponInfo> availableWeapons = from w in Data.MainWeapons
                                                           where w.Rank <= level
                                                           select w;
            MainWeaponInfo randomWeapon = availableWeapons.ElementAt(random.Next(availableWeapons.Count()));

            GuildEmote subEmote;
            GuildEmote specialEmote;
            await Context.Channel.SendMessageAsync("Through the fun process of pseudo-random number generation, I've decided that you should use this weapon!", embed: randomWeapon.GetDiscordEmbed(userInfo.Locale, out subEmote, out specialEmote, Context.Guild));

            // Currently crashes the bot???
            //await Task.Delay(5000);
            //if (subEmote != null) await Context.Guild.DeleteEmoteAsync(subEmote);
            //if (specialEmote != null) await Context.Guild.DeleteEmoteAsync(specialEmote);
        }
    }
}
