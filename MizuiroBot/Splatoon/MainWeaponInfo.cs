using System;
using System.Collections.Generic;
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
    }
}
