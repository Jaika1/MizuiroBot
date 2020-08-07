using Discord;
using MizuiroBot.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MizuiroBot.Splatoon
{
    public class SpecialWeaponInfo
    {
        /// <summary>
        /// The internal ID for the given sub weapon.
        /// </summary>
        public int Id = -1;
        /// <summary>
        /// Defines the <b>internal name</b> for this special weapon. (Not its given name, use the locale functions instead!)
        /// </summary>
        public string Name = "";

        public string GetName(LocaleSetting locale)
        {
            try
            {
                return Data.Locales[(int)locale].First(x => x.Key == Name).Value;
            }
            catch
            {
                return "ERROR!";
            }
        }

        public string GetImageUrl()
        {
            return $"https://leanny.github.io/splat2/subspe/Wsp_{Name}.png";
        }

        public Embed GetDiscordEmbed(LocaleSetting locale)
        {
            return new EmbedBuilder()
                .WithTitle(GetName(locale))
                .WithThumbnailUrl(GetImageUrl())
                .WithColor(new Color(0xFF, 0x90, 0x00))
                .Build();
        }
    }
}
