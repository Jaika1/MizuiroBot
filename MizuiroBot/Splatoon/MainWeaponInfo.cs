using Discord;
using Discord.WebSocket;
using MizuiroBot.Shared;
using MizuiroBot.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MizuiroBot.Splatoon
{
    public class MainWeaponInfo
    {
        /// <summary>
        /// What version of the game this weapon was introduced in.
        /// </summary>
        public int Addition = 0;
        /// <summary>
        /// The internal ID for this weapon.
        /// </summary>
        public int Id = -1;
        /// <summary>
        /// The level of effect that Ink Saver (Main) has on this weapon.
        /// </summary>
        public string InkSaverLv = "";
        /// <summary>
        /// I'm not too sure on this one...?
        /// </summary>
        public string Lock = "";
        /// <summary>
        /// Defines the type of effect that Main Power Up has on this weapon
        /// </summary>
        public string MainUpGearPowerType = "";
        /// <summary>
        /// Defines the underlying weight class of this weapon.
        /// </summary>
        public string MoveVelLv = "";
        /// <summary>
        /// Defines the <b>internal name</b> for this weapon. (Not its given name, use the locale functions instead!)
        /// </summary>
        public string Name = "";
        /// <summary>
        /// What the first parameter represents internally. (Use locale functions for clarification!)
        /// </summary>
        public string Param0 = "";
        /// <summary>
        /// What the second parameter represents internally. (Use locale functions for clarification!)
        /// </summary>
        public string Param1 = "";
        /// <summary>
        /// What the third parameter represents internally. (Use locale functions for clarification!)
        /// </summary>
        public string Param2 = "";
        /// <summary>
        /// What the first parameters actual value is. (Use locale functions for clarification!)
        /// </summary>
        public int ParamValue0 = 0;
        /// <summary>
        /// What the second parameters actual value is. (Use locale functions for clarification!)
        /// </summary>
        public int ParamValue1 = 0;
        /// <summary>
        /// What the third parameters actual value is. (Use locale functions for clarification!)
        /// </summary>
        public int ParamValue2 = 0;
        /// <summary>
        /// What the cost of the weapon is in-game.
        /// </summary>
        public int Price = 0;
        /// <summary>
        /// The in-game parameter for the weapons range as is shown to the user?
        /// </summary>
        public int Range = 0;
        /// <summary>
        /// What level a player must be at before being able to unlock this weapon.
        /// </summary>
        public int Rank = 1;
        /// <summary>
        /// Seems like another internal value that I am unsure of.
        /// </summary>
        public string ShotMoveVelType = "";
        /// <summary>
        /// Defines the <b>internal name</b> for the special weapon this weapon comes equipped with. (Not its given name, use the locale functions instead!)
        /// </summary>
        public string Special = "";
        /// <summary>
        /// How many points a player must earn before the weapons special is ready.
        /// </summary>
        public int SpecialCost = 0;
        /// <summary>
        /// Yet another internal value I know not much about.
        /// </summary>
        public string StealthMoveAccLv = "";
        /// <summary>
        /// Defines the <b>internal name</b> for the sub-weapon this weapon comes equipped with. (Not its given name, use the locale functions instead!)
        /// </summary>
        public string Sub = "";

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
            return $"https://leanny.github.io/splat2/weapons/Wst_{Name}.png";
        }

        public Embed GetDiscordEmbed(LocaleSetting locale, out GuildEmote subEmote, out GuildEmote specialEmote, SocketGuild guild)
        {
            try
            {
                Image img = new Image(WebRequest.Create(GetSubWeapon().GetImageUrl()).GetResponse().GetResponseStream());
                subEmote = guild.CreateEmoteAsync(Name, img).GetAwaiter().GetResult();
            }
            catch
            {
                subEmote = null;
            }

            try
            {
                Image img = new Image(WebRequest.Create(GetSpecialWeapon().GetImageUrl()).GetResponse().GetResponseStream());
                specialEmote = guild.CreateEmoteAsync(Name, img).GetAwaiter().GetResult();
            }
            catch
            {
                specialEmote = null;
            }

            Embed eb = new EmbedBuilder()
                .WithTitle(GetName(locale))
                .WithThumbnailUrl(GetImageUrl())
                .WithColor(new Color(0x00, 0xFF, 0x00))
                .AddField("Sub Weapon", (subEmote==null ? "" : $"{subEmote} ") + $"*{GetSubWeapon().GetName(locale)}*")
                .AddField("Special", (specialEmote == null ? "" : $"{specialEmote} ") + $"*{GetSpecialWeapon().GetName(locale)}*")
                .AddField(Data.Locales[(int)locale].First(x => x.Key == Param0).Value, $"{ASCIIUI.CreateTextBar(ParamValue0, 100)} **[{ParamValue0}/100]**")
                .AddField(Data.Locales[(int)locale].First(x => x.Key == Param1).Value, $"{ASCIIUI.CreateTextBar(ParamValue1, 100)} **[{ParamValue1}/100]**")
                .AddField(Data.Locales[(int)locale].First(x => x.Key == Param2).Value, $"{ASCIIUI.CreateTextBar(ParamValue2, 100)} **[{ParamValue2}/100]**")
                .Build();

            return eb;
        }

        public SubWeaponInfo GetSubWeapon()
        {
            try
            {
                return Data.SubWeapons.First(x => x.Name == Sub);
            } catch
            {
                return null;
            }
        }

        public SpecialWeaponInfo GetSpecialWeapon()
        {
            try
            {
                return Data.SpecialWeapons.First(x => x.Name == Special);
            }
            catch
            {
                return null;
            }
        }
    }
}
