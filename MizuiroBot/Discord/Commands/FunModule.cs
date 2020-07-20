﻿using Discord;
using Discord.Commands;
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
            IEnumerable<MainWeaponInfo> availableWeapons = from w in Data.MainWeapons
                                                           where w.Rank <= level
                                                           select w;
            MainWeaponInfo randomWeapon = availableWeapons.ElementAt(random.Next(availableWeapons.Count()));
            await Context.Channel.SendMessageAsync($"Through a process of pseudo-random selection, I've determined that the `{randomWeapon.GetName()}` is the weapon for you at this current moment!");
        }
    }
}
