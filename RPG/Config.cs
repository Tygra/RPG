#region Disclaimer
/*  
 *  The plugin has some features which I got from other authors.
 *  I don't claim any ownership over those elements which were made by someone else.
 *  The plugin has been customized to fit our need at Geldar,
 *  and because of this, it's useless for anyone else.
 *  I know timers are shit, and If someone knows a way to keep them after relog, tell me.
*/
#endregion

#region Refs
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
#endregion

namespace RPG
{
    public class Config
    {
        public static Contents contents;

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

                #region Housing
            
                public int altcost = 5000;
                public string defaultowner = "Tygra";
                public string abovehousing = "abovehousing";
                public string underhousing = "uhousing";
                
                public string h1region = "h1";
                public int h1cost = 5000;
            /*
                public string h2region = "h2";
                public int h2cost = ;

                public string h3region = "h3";
                public int h3cost = ;

                public string h4region = "h4";
                public int h4cost = ;

                public string h5region = "h5";
                public int h5cost = ;

                public string h6region = "h6";
                public int h6cost = ;

                public string h7region = "h7";
                public int h7cost = ;

                public string h8region = "h8";
                public int h8cost = ;

                public string h9region = "h9";
                public int h9cost = ;

                public string h10region = "h10";
                public int h10cost = ;
                */
            #endregion

                public int mimicchest = 48;
                public int mimichallowkey = 3092;
                public int mimiccrimsonkey = 3091;
                public int mimiccorruptkey = 3091;

                public int mimichallow = 475;
                public int mimiccorrupt = 473;
                public int mimiccrimson = 474;
                
                public string pyramid1region = "pyramid1";
                public int pyramid1item1 = 327;
                public int pyramid1item2 = 159;
                public int pyramid1item3 = 790;
                public int pyramid1cd = 172800;

                public string pyramid2region = "pyramid2";
                public int pyramid2item1 = 327;
                public int pyramid2item2 = 791;
                public int pyramid2item3 = 116;
                public int pyramid2cd = 172800;

                public string pyramid3region = "pyramid3";
                public int pyramid3item1 = 327;
                public int pyramid3item2 = 285;
                public int pyramid3item3 = 789;
                public int pyramid3item4 = 188;
                public int pyramid3cd = 172800;

                public string pyramid4region = "pyramid4";
                public int pyramid4item1 = 327;
                public int pyramid4item2 = 857;
                public int pyramid4item3 = 791;
                public int pyramid4item4 = 790;
                public int pyramid4cd = 172800;

                public string pyramid5region = "pyramid5";
                public int pyramid5item1 = 327;
                public int pyramid5item2 = 848;
                public int pyramid5item3 = 866;
                public int pyramid5item4 = 188;
                public int pyramid5cd = 172800;

                public string pyramid6region = "pyramid6";
                public int pyramid6item1 = 327;
                public int pyramid6item2 = 49;
                public int pyramid6item3 = 791;
                public int pyramid6cd = 172800;

                public string pyramid7region = "pyramid7";
                public int pyramid7item1 = 327;
                public int pyramid7item2 = 159;
                public int pyramid7item3 = 790; 
                public int pyramid7cd = 172800;

                public string pyramid8region = "pyramid8";
                public int pyramid8item1 = 327;
                public int pyramid8item2 = 159;
                public int pyramid8item3 = 790;
                public int pyramid8cd = 172800;

                public string ice1region = "ice1";
                public int ice1item1 = 327;
                public int ice1item2 = 1319;
                public int ice1item3 = 188;
                public int ice1cd = 172800;

                public string ice2region = "ice2";
                public int ice2item1 = 327;
                public int ice2item2 = 950;
                public int ice2item3 = 188;
                public int ice2cd = 172800;

                public string ice3region = "ice3";
                public int ice3item1 = 327;
                public int ice3item2 = 987;
                public int ice3item3 = 188;
                public int ice3cd = 172800;

                public string ice4region = "ice4";
                public int ice4item1 = 327;
                public int ice4item2 = 724;
                public int ice4item3 = 188;
                public int ice4cd = 172800;

                public string ice5region = "ice5";
                public int ice5item1 = 327;
                public int ice5item2 = 997;
                public int ice5item3 = 188;
                public int ice5cd = 172800;

                public string ice6region = "ice6";
                public int ice6item1 = 327;
                public int ice6item2 = 670;
                public int ice6item3 = 188;
                public int ice6cd = 172800;

                public string corr1region = "corr1";
                public int corr1item1 = 327;
                public int corr1item2 = 522;
                public int corr1cd = 172800;

                public string corr2region = "corr2";
                public int corr2item1 = 327;
                public int corr2item2 = 556;
                public int corr2item3 = 499;
                public int corr2cd = 172800;

                public string corr3region = "corr3";
                public int corr3item1 = 327;
                public int corr3item2 = 1225;
                public int corr3cd = 172800;

                public string corr4region = "corr4";
                public int corr4item1 = 327;
                public int corr4item2 = 1819;
                public int corr4item3 = 1820;
                public int corr4cd = 172800;

                public string crim1region = "crim1";
                public int crim1item1 = 327;
                public int crim1item2 = 556;
                public int crim1item3 = 499;
                public int crim1cd = 172800;

                public string crim2region = "crim2";
                public int crim2item1 = 327;
                public int crim2item2 = 1225;
                public int crim2cd = 172800;

                public string crim3region = "crim3";
                public int crim3item1 = 327;
                public int crim3item2 = 1332;
                public int crim3cd = 172800;

                public string crim4region = "crim4";
                public int crim4item1 = 327;
                public int crim4item2 = 1838;
                public int crim4item3 = 1839;
                public int crim4item4 = 1840;           
                public int crim4cd = 172800;

                public string jadv1region = "jadv1";
                public int jadv1item1 = 327;
                public int jadv1item2 = 3360;
                public int jadv1item3 = 3361;
                public int jadv1cd = 172800;

                public string jadv2region = "jadv2";
                public int jadv2item1 = 327;
                public int jadv2item2 = 753;
                public int jadv2cd = 172800;

                public string jadv3region = "jadv3";
                public int jadv3item1 = 327;
                public int jadv3item2 = 3068;
                public int jadv3cd = 172800;

                public string jadv4region = "jadv4";
                public int jadv4item1 = 327;
                public int jadv4item2 = 964;
                public int jadv4cd = 172800;

                public string jadv5region = "jadv5";
                public int jadv5item1 = 327;
                public int jadv5item2 = 2204;
                public int jadv5cd = 172800;

                public string space1region = "space1";
                public int space1item1 = 327;
                public int space1item2 = 117;
                public int space1item3 = 179;
                public int space1cd = 172800;

                public string space2region = "space2";
                public int space2item1 = 327;
                public int space2item2 = 75;
                public int space2item3 = 181;
                public int space2cd = 172800;

                public string space3region = "space3";
                public int space3item1 = 327;
                public int space3item2 = 75;
                public int space3item3 = 177;
                public int space3cd = 172800;

                public string space4region = "space4";
                public int space4item1 = 327;
                public int space4item2 = 117;
                public int space4item3 = 178;
                public int space4cd = 172800;

                public string hallow1region = "hallow1";
                public int hallow1item1 = 327;
                public int hallow1item2 = 1725;
                public int hallow1item3 = 499;
                public int hallow1cd = 172800;

                public string hallow2region = "hallow2";
                public int hallow2item1 = 327;
                public int hallow2item2 = 1725;
                public int hallow2item3 = 3548;
                public int hallow2cd = 172800;

                public string hallow3region = "hallow3";
                public int hallow3item1 = 327;
                public int hallow3item2 = 1774;
                public int hallow3item3 = 499;
                public int hallow3cd = 432000;

                public string hallow4region = "hallow4";
                public int hallow4item1 = 327;
                public int hallow4item2 = 1725;
                public int hallow4item3 = 499;
                public int hallow4cd = 172800;

                public string hallow5region = "hallow5";
                public int hallow5item1 = 327;
                public int hallow5item2 = 1725;
                public int hallow5item3 = 499;
                public int hallow5cd = 172800;

                public string pyramidtpregion = "pyramidtp";
                public string spacetpregion = "spacetp";
                public string corrtpregion = "corrtp";
                public string crimtpregion = "crimtp";
                public string icetpregion = "icetp";
                public string hallowtpregion = "hallowtp";
                public string jadvtpregion = "jadvtp";

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

                public string qlab1region = "sideq2";
                public int qlab1cd = 259200;
                public int qlab1reward = 150;

                public string qlab2region = "sideq3";
                public int qlab2cd = 259200;
                public int qlab2reward = 150;

                public string qlab3region = "sideq4";
                public int qlab3cd = 259200;
                public int qlab3reward = 150;

                public string shrine1region = "shrine1";
                public int shrine1cd = 259200;
                public int shrine1reward = 75;

                public string shrine2region = "shrine2";
                public int shrine2cd = 259200;
                public int shrine2reward = 75;

                public string shrine3region = "shrine3";
                public int shrine3cd = 259200;
                public int shrine3reward = 75;

                public string shrine4region = "shrine4";
                public int shrine4cd = 259200;
                public int shrine4reward = 75;

                public string vikingregion = "viking";
                public int vikingcd = 259200;
                public int vikingreward = 100;
                public int vikingitem1 = 879;

                public string vulcanregion = "volcano";
                public int vulcancd = 259200;
                public int vulcanreward = 150;
                public int vulcanitem1 = 173;

                public string shrine5region = "shrine5";
                public int shrine5cd = 259200;
                public int shrine5reward = 75;

                public string storyregion = "spawn";

                public string greekoneregion = "greekone";
                public int greekoneitem = 3199;
                public int greekonecd = 604800;

                public string caveregion = "greektwo";
                public int caveitem1 = 19;
                public int caveitem2 = 3520;
                public int cavecd = 604800;

                public string corruptedregion = "corrupted";
                public int corrupteditem = 70;
                public int corruptedreward = 75;
                public int corruptedcd = 259200;
                
                public string frozenregion = "frozen";
                public int frozenitem = 1319;
                public int frozenreward = 75;
                public int frozencd = 259200;
            
                public string hiveregion = "hive";
                public int hiverewarditem = 1133;
                public int hivenpcamount1 = 10;
                public int hivenpcamount2 = 10;
                public int hivenpcid1 = 211;
                public int hivenpcid2 = 210;
                public int hivecd = 21600;
                public int hivereqitem = 1132;        
            
                public string highlanderregion = "highlander";
                public int highlanderitem = 2273;
                public int highlanderreward = 75;
                public int highlandercd = 259200;
                
                public string overgrownregion = "overgrown";
                public int overgrownitem = 1133;
                public int overgrownreward = 100;
                public int overgrowncd = 259200;

                public string dungeonregion = "dungeon";
                public int dungeonitem1 = 154;
                public int dungeonitem2 = 327;
                public int dungeonreward = 75;
                public int dungeoncd = 259200;
            /*
            public string hellq1region = "hellq1";
            public int hellq1item1 = ;
            public int hellq1item2 = ;
            public int hellq1reward = ;
            public int hellq1cd = 259200;

            public string hellq2region = "hellq2";
            public int hellq2item1 = ;
            public int hellq2item2 = ;
            public int hellq2reward = ;
            public int hellq2cd 259200 ;

            public string hunter1region = "hunter1";
            public int hunter1item1 = ;
            public int hunter2item2 = ;
            public int hunter1cd = 259200;

            public string hunter2region = "hunter2";
            public int hunter2item1 = ;
            public int hunter2item2 = ;
            public int hunterreward = ;
            public int hunter2cd = 259200;
            */
                public string lab1region = "lab1";
                public int lab1reward = 68;

                public string lab2region = "lab2";
                public int lab2cd = 604800;

                public string hiddenregion = "hidden";
                public int hiddenitem = 1579;
                public int hiddencd = 640800;

                public string millregion = "mill";
                public int millitem = 1238;
                public int millcd = 604800;

                public int facepalmcd = 30;
                public int slapallcd = 3600;

                public int giftcd = 14400;
                public string giftregion = "gift";
                public int giftitem1 = 1922;
                public int giftitem2 = 1869;
                public int giftreward = 50;

                public int startermage = 3069;
                public int starterwarrior = 280;
                public int starterranger = 3492;
                public int startersummoner = 1309;
                public int startercd = 10800;

                public int buff1cd = 60;
                public int buff2cd = 60;
                public int buff3cd = 60;
                public int buff4cd = 60;
                public int buff1cost = 150;
                public int buff2cost = 200;
                public int buff3cost = 350;
                public int buff4cost = 500;

                public int mgcd = 120;
                public int mgcost = 1000;
                public int[] mgexlcude = { 17, 18, 19, 20, 22, 35, 37, 38, 54, 68, 85, 105, 106, 107, 108, 113, 114, 113, 123, 124, 125, 126, 127, 128, 129, 130, 131, 134, 135, 136, 139, 142, 143, 144, 145, 158, 159, 160, 162, 166, 172, 178, 207, 208, 209, 212, 213, 214, 215, 216, 227, 228, 229, 245, 246, 247, 248, 251, 253, 262, 263, 264, 265, 269, 276, 281, 282, 288, 290, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 325, 326, 327, 328, 338, 339, 340, 344, 345, 346, 353, 354, 368, 369, 370, 372, 373, 376, 379, 380, 381, 382, 383, 385, 386, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 422, 437, 438, 439, 440, 441, 453, 460, 461, 462, 463, 466, 467, 468, 473, 474, 475, 476, 477, 491, 492, 493, 507, 517, 521, 2889, 2890, 2891, 2892, 2893, 2894, 2895, 3564 };

                public int trial30item1 = 1071;
                public int trial30item2 = 327;
                public int trial30item3 = 2349;
                public string trial30region = "trial";
                public string trial30magegroup = "mage29";
                public string trial30warriorgroup = "warrior29";
                public string trial30rangergroup = "ranger29";
                public string trial30summonergroup = "summoner29";
                public string trial30magefinish = "mage30";
                public string trial30warriorfinish = "warrior30";
                public string trial30rangerfinish = "ranger30";
                public string trial30summonerfinish = "summoner30";
                public string trial30terrariangroup = "terrarian29";
                public string trial30terrarianfinish = "terrarian30";
                public string lab1magegroup = "mage29_1";
                public string lab1rangergroup = "ranger29_1";
                public string lab1warriorgroup = "warrior29_1";
                public string lab1summonergroup = "summoner29_1";
                public string lab1terrariangroup = "terrarian29_1";
                public string lab2magegroup = "mage29_2";
                public string lab2rangergroup = "ranger29_2";
                public string lab2warriorgroup = "warrior29_2";
                public string lab2summonergroup = "summoner29_2";
                public string lab2terrariangroup = "terrarian29_2";
                public string trialskipregion = "trialskip";
                public int trial30skipcost = 50000;
                public int trial60skipcost = 200000;
                public int trial70skipcost = 35000;
                public int trial30hintcost = 500;
                public int trial60hintcost = 7500;

                public int lab1npc1 = 13;
                public int lab1npc2 = 95;
                public int lab2npc1 = 513;
                public int lab2npc2 = 509;
                public int trial30npc = 51;

                public string trial60shrineregion = "trial60pre";
                public string trial60mageshrinegroup = "mage59";
                public string trial60warriorshrinegroup = "warrior59";
                public string trial60rangershrinegroup = "ranger59";
                public string trial60summonershrinegroup = "summoner59";
                public string trial60terrarianshrinegroup = "terrarian59"; 
                public string trial60mageregion = "trial60mage";
                public string trial60warriorregion = "trial60warrior";
                public string trial60rangerregion = "trial60ranger";
                public string trial60summonerregion = "trial60summoner";
                public string trial60terrarianregion = "trial60ranger";
                public string trial60magegroup = "mage59_1";
                public string trial60rangergroup = "ranger59_1";
                public string trial60warriorgroup = "warrior59_1";
                public string trial60summonergroup = "summoner59_1";
                public string trial60terrariangroup = "terrarian59_1";
                public string trial60magefinish = "mage60";
                public string trial60warriorfinish = "warrior60";
                public string trial60rangerfinish = "ranger60";
                public string trial60summonerfinish = "summoner60";
                public string trial60terrarianfinish = "terrarian60";
                public int trial60cost = 12000;
                public string trial60questionregion = "trial60question";

                public string trial70magegroup = "mage69";
                public string trial70warriorgroup = "warrior69";
                public string trial70rangergroup = "ranger69";
                public string trial70summonergroup = "summoner69";
                public string trial70terrariangroup = "terrarian69";
                public string trial70magefinish = "mage70";
                public string trial70rangerfinish = "ranger70";
                public string trial70warriorfinish = "warrior70";
                public string trial70summonerfinish = "summoner70";
                public string trial70terrarianfinish = "terrarian70";

            
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
