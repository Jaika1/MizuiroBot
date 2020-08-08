using AsyncTwitchLib;
using MizuiroBot.Shared;
using MizuiroBot.Splatoon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizuiroBot.Twitch
{
    class FunModule : TwitchCommandModule
    {
        private Random random = new Random();

        [TwitchCommand("randomweapon")]
        public async Task RandomWeaponCommand(int level = 30)
        {
            IEnumerable<MainWeaponInfo> availableWeapons = from w in Data.MainWeapons
                                                           where w.Rank <= level
                                                           select w;
            MainWeaponInfo randomWeapon = availableWeapons.ElementAt(random.Next(availableWeapons.Count()));

            await Channel.SendChatMessage($"Through the fun process of pseudo-random number generation, I've decided that you should use the {randomWeapon.GetName(LocaleSetting.English)}! It comes equipped with {randomWeapon.GetSubWeapon().GetName(LocaleSetting.English)} and {randomWeapon.GetSpecialWeapon().GetName(LocaleSetting.English)}!");
        }
    }
}
