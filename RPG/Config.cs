using System;
using System.IO;
using System.ComponentModel;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using TShockAPI.DB;
using Wolfje.Plugins.SEconomy;
using Wolfje.Plugins.SEconomy.Journal;
using Newtonsoft.Json;

namespace RPG
{
    public class Config
    {
        public static Contents contents;

        public static Trial60 trial60;

        #region Config create

        public static void CreateConfig()
        {
            string filepath = Path.Combine(TShock.SavePath, "rpg.json");
            try
            {
                using (var stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (var sr = new StreamWriter(stream))
                    {
                        contents = new Contents();
                        var configString = JsonConvert.SerializeObject(contents, Formatting.Indented);
                        sr.Write(configString);
                    }
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError(ex.Message);
            }
        }

        #endregion

        #region Config read

        public static bool ReadConfig()
        {
            string filepath = Path.Combine(TShock.SavePath, "rpg.json");
            try
            {
                if (File.Exists(filepath))
                {
                    using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            var configString = sr.ReadToEnd();
                            contents = JsonConvert.DeserializeObject<Contents>(configString);
                        }
                        stream.Close();
                    }
                    return true;
                }
                else
                {
                    TShock.Log.ConsoleError("RPG Config not found, how about a new one...");
                    CreateConfig();
                    return true;
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError(ex.Message);
            }
            return false;
        }

        #endregion

        #region Config

            public class Contents
            {
                public bool SEconomy = true;

                public string pyramid1region = "pyramid1";
                public int pyramid1cd = 172800;

                public string pyramid2region = "pyramid2";
                public int pyramid2cd = 172800;

                public string pyramid3region = "pyramid3";
                public int pyramid3cd = 172800;

                public string pyramid4region = "pyramid4";
                public int pyramid4cd = 172800;

                public string pyramid5region = "pyramid5";
                public int pyramid5cd = 172800;

                public string pyramid6region = "pyramid6";
                public int pyramid6cd = 172800;

                public string pyramid7region = "pyramid7";
                public int pyramid7cd = 172800;

                public string pyramid8region = "pyramid8";
                public int pyramid8cd = 172800;

                public string ice1region = "ice1";
                public int ice1cd = 172800;

                public string ice2region = "ice2";
                public int ice2cd = 172800;

                public string ice3region = "ice3";
                public int ice3cd = 172800;

                public string ice4region = "ice4";
                public int ice4cd = 172800;

                public string ice5region = "ice5";
                public int ice5cd = 172800;

                public string ice6region = "ice6";
                public int ice6cd = 172800;

                public string corr1region = "corr1";
                public int corr1cd = 172800;

                public string corr2region = "corr2";
                public int corr2cd = 172800;

                public string corr3region = "corr3";
                public int corr3cd = 172800;

                public string corr4region = "corr4";
                public int corr4cd = 172800;

                public string crim1region = "crim1";
                public int crim1cd = 172800;

                public string crim2region = "crim2";
                public int crim2cd = 172800;

                public string crim3region = "crim3";
                public int crim3cd = 172800;

                public string crim4region = "crim4";
                public int crim4cd = 172800;

                public string jadv1region = "jadv1";
                public int jadv1cd = 172800;

                public string jadv2region = "jadv2";
                public int jadv2cd = 172800;

                public string jadv3region = "jadv3";
                public int jadv3cd = 172800;

                public string jadv4region = "jadv4";
                public int jadv4cd = 172800;

                public string jadv5region = "jadv5";
                public int jadv5cd = 172800;

                public string space1region = "space1";
                public int space1cd = 172800;

                public string space2region = "space2";
                public int space2cd = 172800;

                public string space3region = "space3";
                public int space3cd = 172800;

                public string space4region = "space4";
                public int space4cd = 172800;

                public string hallow1region = "hallow1";
                public int hallow1cd = 172800;

                public string hallow2region = "hallow2";
                public int hallow2cd = 172800;

                public string hallow3region = "hallow3";
                public int hallow3cd = 432000;

                public string hallow4region = "hallow4";
                public int hallow4cd = 172800;

                public string hallow5region = "hallow5";
                public int hallow5cd = 172800;

                public string tutclassregion = "tutclass";
                public string tutgearregion = "tutgear";
                public string tutmineregion = "tutmine";
                public string tuttraderegion = "tuttrade";
                public string tuthouseregion = "tuthouse";
                public string dropretryregion = "dropretry";
                public string tutdropregion = "tutdrop";

                public string oasisregion = "oasis";
                public int oasiscost = 250;

                public string giroregion = "sideq1";
                public int girocd = 259200;
                public int giroreward = 150;

                public string storyregion = "spawn";

                public string greekoneregion = "greekone";
                public int greekonecd = 604800;

                public string caveregion = "greektwo";
                public int cavecd = 604800;

                public string lab1region = "lab1";
                public int lab1cd = 604800;

                public string lab2region = "lab2";
                public int lab2cd = 604800;

                public string hiddenregion = "hidden";
                public int hiddencd = 640800;

                public string millregion = "mill";
                public int millcd = 604800;

                public int facepalmcd = 30;
                public int slapallcd = 3600;

                public string trial30region = "trial";
                public string trial30magegroup = "mage29";
                public string trial30warriorgroup = "warrior29";
                public string trial30rangergroup = "ranger29";
                public string trial30summonergroup = "summoner29";
                public string trialskipregion = "trialskip";
                public int trial30skipcost = 50000;
                public int trial60skipcost = 200000;                
            }
            public class Trial60
            {
                public string trial60mageregion = "trial60mage";
                public string trial60warriorregion = "trial60warrior";
                public string trial60rangerregion = "trial60ranger";
                public string trial60summonerregion = "trial60summoner";
                public int trial60cost = 12000;
            }

        #endregion

        #region Config reload

        private void Reloadcfg(CommandArgs args)
        {
            if (ReadConfig())
            {
                args.Player.SendMessage("RPG config reloaded.", Color.Goldenrod);
            }

            else
            {
                args.Player.SendErrorMessage("Nope. Check logs.");
            }
        }

        #endregion
        
    }
}
