using System;
using System.Collections.Generic;
using System.Text;

namespace MizuiroBot.Splatoon
{
    public struct MainWeaponInfo
    {
        private int subWeaponIndex;
        private int specialWeaponIndex;

        public string ImageUrl;
        public string Name;
        public MainWeaponClass WeaponClass;
        public SubWeaponInfo SubWeapon => Data.SubWeapons[subWeaponIndex];
        public SpecialWeaponInfo SpecialWeapon => Data.SpecialWeapons[specialWeaponIndex];
        public byte Level;
        public ushort Cost;
        public ushort BaseDamage;
        public float InkConsumption;
        public byte SpecialPoints;
    }

    public enum MainWeaponClass
    {
        Shooter,
        Roller,
        Charger,
        Slosher,
        Splatling,
        Dualie,
        Brella,
        Rare
    }
}
