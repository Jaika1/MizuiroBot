using MizuiroBot.Shared;
using MizuiroBot.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace MizuiroBot.Splatoon
{
    public static class Data
    {
        private const string dataLocation = @".\data\";
        private static WebClient webClient = new WebClient();
        private const string s2DataUrl = "https://github.com/Leanny/leanny.github.io/archive/master.zip"; //There is a better way to do this, will be implemented in the future.

        private const string englishLocaleLocation = dataLocation + @"Languages\lang_dict_USen.json";
        private const string germanLocaleLocation = dataLocation + @"Languages\lang_dict_EUde.json";
        private const string spanishLocaleLocation = dataLocation + @"Languages\lang_dict_EUes.json";
        private const string frenchLocaleLocation = dataLocation + @"Languages\lang_dict_EUfr.json";
        private const string italianLocaleLocation = dataLocation + @"Languages\lang_dict_EUit.json";
        private const string dutchLocaleLocation = dataLocation + @"Languages\lang_dict_EUnl.json";
        private const string japaneseLocaleLocation = dataLocation + @"Languages\lang_dict_JPja.json";
        private const string russianLocaleLocation = dataLocation + @"Languages\lang_dict_EUru.json";
        public static Dictionary<string, string>[] Locales = new Dictionary<string, string>[8];

        private const string mainWeaponLocation = dataLocation + @"Mush\latest\WeaponInfo_Main.json";
        private const string subWeaponLocation = dataLocation + @"Mush\latest\WeaponInfo_Sub.json";
        private const string specialWeaponLocation = dataLocation + @"Mush\latest\WeaponInfo_Special.json";

        public static List<MainWeaponInfo> MainWeapons;
        public static List<SubWeaponInfo> SubWeapons;
        public static List<SpecialWeaponInfo> SpecialWeapons;

        public static void LoadSplatoon2Data()
        {
            CVTS.WriteLineInfo($"Attempting to load Splatoon 2 data...");
            if (!Directory.Exists(dataLocation))
            {
                CVTS.WriteLineInfo("Splatoon 2 data not found! An attempt to download it will be made.");
                Directory.CreateDirectory(dataLocation);
                DownloadSplatoon2Data();
            }

            #region LOAD IN ALL THE DATA!
            // Load in each locale
            CVTS.WriteLineInfo("Loading in the en-us locale for Splatoon 2...");
            Locales[(int)LocaleSetting.English] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(englishLocaleLocation));
            CVTS.WriteLineInfo("Loading in the de-eu locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Deutsche] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(germanLocaleLocation));
            CVTS.WriteLineInfo("Loading in the es-eu locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Espanol] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(spanishLocaleLocation));
            CVTS.WriteLineInfo("Loading in the fr-eu locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Francais] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(frenchLocaleLocation));
            CVTS.WriteLineInfo("Loading in the it-eu locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Italiano] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(italianLocaleLocation));
            CVTS.WriteLineInfo("Loading in the nl-eu locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Nederlands] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(dutchLocaleLocation));
            CVTS.WriteLineInfo("Loading in the ja-jp locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Nihongo] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(japaneseLocaleLocation));
            CVTS.WriteLineInfo("Loading in the ru-eu locale for Splatoon 2...");
            Locales[(int)LocaleSetting.Russkiy] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(russianLocaleLocation));

            //#if DEBUG
            //            foreach (KeyValuePair<string, string> p in EnglishLocale)
            //                CVTS.WriteLine($"{p.Key,34}: {p.Value.Replace("\n", "\\n"),100}", Color.FromArgb(0x00, 0xFF, 0x00), "LOCALE");
            //#endif

            // Load in the main weapons
            MainWeapons = JsonConvert.DeserializeObject<List<MainWeaponInfo>>(File.ReadAllText(mainWeaponLocation));
#if DEBUG
            foreach (MainWeaponInfo m in MainWeapons)
                CVTS.WriteLine($"{m.Name,30}: {m.GetName(LocaleSetting.English)}", Color.FromArgb(0xFF, 0x00, 0x99), "MAINWEPS");
#endif

            // Load in the sub weapons
            SubWeapons = JsonConvert.DeserializeObject<List<SubWeaponInfo>>(File.ReadAllText(subWeaponLocation));
#if DEBUG
            foreach (SubWeaponInfo m in SubWeapons)
                CVTS.WriteLine($"{m.Name,30}: {m.GetName(LocaleSetting.English)}", Color.FromArgb(0xFF, 0x77, 0x99), "SUBWEPS");
#endif

            // Load in the special weapons
            SpecialWeapons = JsonConvert.DeserializeObject<List<SpecialWeaponInfo>>(File.ReadAllText(specialWeaponLocation));
#if DEBUG
            foreach (SpecialWeaponInfo m in SpecialWeapons)
                CVTS.WriteLine($"{m.Name,30}: {m.GetName(LocaleSetting.English)}", Color.FromArgb(0xFF, 0xCC, 0x99), "SPECIALS");
#endif
            #endregion

            // Check to see if any weapons have actually been loaded in.
            if (MainWeapons == null || MainWeapons.Count == 0)
                CVTS.WriteLineWarn("No main weapons have been loaded, certain commands will not function correctly.");
            if (SubWeapons == null || SubWeapons.Count == 0)
                CVTS.WriteLineWarn("No sub weapons have been loaded, certain commands will not function correctly.");
            if (SpecialWeapons == null || SpecialWeapons.Count == 0)
                CVTS.WriteLineWarn("No special weapons have been loaded, certain commands will not function correctly.");
            CVTS.WriteLineOk("Finished aquiring all Splatoon 2 data.");
        }

        private static void DownloadSplatoon2Data()
        {
            CVTS.WriteLineInfo($"Downloading repository containing Splatoon 2 data from {s2DataUrl}...");
            try
            {
                string masterZipPath = $"{dataLocation}master.zip";
                webClient.DownloadFile(s2DataUrl, masterZipPath);
                CVTS.WriteLineOk($"Download succeeded, extracting data folder...");
                using (ZipArchive archive = ZipFile.OpenRead(masterZipPath))
                {
                    var result = from currEntry in archive.Entries
                                 where Path.GetDirectoryName(currEntry.FullName).Contains(@"leanny.github.io-master\data")
                                 where !string.IsNullOrEmpty(currEntry.Name)
                                 select currEntry;


                    foreach (ZipArchiveEntry entry in result)
                    {
                        string path = Path.Combine(dataLocation, entry.FullName.Replace(@"leanny.github.io-master/data/", null));
                        CVTS.WriteLineInfo($"Extracting {path}...");
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        entry.ExtractToFile(path);
                    }
                }
                CVTS.WriteLineInfo($"Deleting master archive...");
                File.Delete(masterZipPath);
                CVTS.WriteLineOk($"Deletion successful!");
            }
            catch
            {
                CVTS.WriteLineError($"Download process failed to complete successfully!");
            }
        }
    }
}
