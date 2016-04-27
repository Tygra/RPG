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
            
                public int altcost = 25000;
                public string defaultowner = "Tygra";
                public int tier1housingindex = 20;
                public int tier2housingindex = 25;
                public int tier3housingindex = 30;
                public int tier4housingindex = 35;
                public int tier5housingindex = 40;
                public int tier6housingindex = 45;

                public int tier6housecost = 25000;
                public int tier5housecost = 30000;
                public int tier4housecost = 40000;
                public int tier3housecost = 35000;
                public int tier2housecost = 45000;
                public int tier1housecost = 50000;

                #region tier1plots array
                public String[] tier1plots = {
                        "h1","h2", "h3", "h4", "h5",
                        "h6","h7","h8","h9","h2",
                        "h11", "h12", "h13","h14", 
                        "h15","h16", "h140","h141",
                        "h142", "h143","h144",
                        "h145", "h146","h147",
                        "h148", "h149","h150",
                        "h151", "h152","h153",
                        "h154", "h155","h156",
                        "h157", "h158", "h159",
                        "h160", "h161", "h162",
                        "h163", "h164", "h165",
                        "h166", "h167", "h168",
                        "h169", "h170", "h171",
                        "h172", "h173", "h174",
                        "h175", "h176", "h177",
                        "h178", "h179", "h180",
                        "h181", "h225", "h226"
                    };
                    #endregion

                #region tier2plots array
                public String[] tier2plots = {
                    "h17","h18","h19",
                    "h20","h21","h22",
                    "h23","h24","h25",
                    "h26","h27","h28",
                    "h29","h30","h31",
                    "h32","h33","h34",
                    "h35","h36","h37",
                    "h38","h39","h40",
                    "h41","h42","h43",
                    "h44","h45","h46",
                    "h47","h48","h49",
                    "h182","h183","h184",
                    "h185","h186","h187",
                    "h188","h189","h190",
                    "h191","h192","h193",
                    "h194","h195","h196",
                    "h197","h198","h199",
                    "h200","h201","h202",
                    "h203","h204","h205",
                    "h206","h207","h208",
                    "h209","h210","h211",
                    "h212","h213","h214",
                    "h215","h216","h217",
                    "h218","h219","h220",
                    "h221","h222","h223","h224"
                };
                #endregion

                #region tier3plots array
                public String[] tier3plots = {
                    "h50", "h51", "h52",
                    "h53", "h54", "h55",
                    "h56", "h57", "h58",
                    "h59", "h60", "h61",
                    "h62", "h80", "h81",
                    "h82", "h83", "h84",
                    "h85", "h86", "h226",
                    "h227", "h228", "h229",
                    "h230", "h231", "h232",
                    "h233", "h234", "h235",
                    "h236", "h237", "h238",
                    "h239", "h240", "h241",
                    "h242", "h243", "h244",
                    "h245", "h246", "h247",
                    "h248", "h249", "h250",
                    "h251", "h252", "h253",
                    "h254", "h255", "h256",
                    "h257", "h258", "h259"
                };
                #endregion

                #region tier4plots array
                public String[] tier4plots = {
                    "h63","h64","h65",
                    "h66","h67","h68",
                    "h69","h70","h71",
                    "h72","h73","h74",
                    "h75","h76","h77",
                    "h78","h79","h87",
                    "h88","h89","h90",
                    "h91","h92","h93",
                    "h94","h95","h96",
                    "h97","h98","h99",
                    "h100","h101","h102",
                    "h103","h104","h105",
                    "h106","h107","h260",
                    "h261","h262","h263",
                    "h264","h265","h266",
                    "h267","h268","h269",
                    "h270","h271","h272",
                    "h273","h274","h275",
                    "h276","h277","h278",
                    "h279","h280","h281",
                    "h282","h283","h284",
                    "h285","h286","h287",
                    "h288","h289","h290",
                    "h291","h292","h293",
                    "h294","h295","h296",
                    "h297","h298","h299",
                    "h300","h301","h302"
                };
                #endregion

                #region tier5plots array
                public String[] tier5plots = {
                    "h108","h109","h110",
                    "h111","h112","h113",
                    "h114","h115","h116",
                    "h117","h118","h119",
                    "h120","h121","h122",
                    "h123","h124","h125",
                    "h126","h127","h128",
                    "h129","h130","h131",
                    "h303","h304","h305",
                    "h306","h307","h308",
                    "h309","h310","h311",
                    "h312","h313","h314"
                };
                #endregion  

                #region tier6plots array
                public String[] tier6plots = {
                    "h131","h132","h133",
                    "h134","h135","h136",
                    "h137","h138","h139"
                };

                #endregion  
            
            #endregion

                public int mimicchest = 48;
                public int mimichallowkey = 3092;
                public int mimiccrimsonkey = 3091;
                public int mimiccorruptkey = 3091;
                public int mimicchestamount = 1;
                public int mimickeyamount = 1;

                public int mimichallow = 475;
                public int mimiccorrupt = 473;
                public int mimiccrimson = 474;

                #region Adventures
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
                #endregion

                public string tutclassregion = "tutclass";
                public string tutgearregion = "tutgear";
                public string tutmineregion = "tutmine";
                public string tuttraderegion = "tuttrade";
                public string tuthouseregion = "tuthouse";
                public string dropretryregion = "dropretry";
                public string tutdropregion = "tutdrop";

                public string oasisregion = "oasis";
                public int oasiscost = 250;
                public string storyregion = "spawn";

                #region Quests
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
                public int hivereqitemamount = 1;        
            
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

                public string hiddenregion = "hidden";
                public int hiddenitem = 1579;
                public int hiddencd = 640800;

                public string millregion = "mill";
                public int millitem = 1238;
                public int millcd = 604800;
                #endregion

                public int facepalmcd = 30;
                public int slapallcd = 3600;

                public int giftcd = 14400;
                public string giftregion = "gift";
                public int giftitem1 = 1922;
                public int giftitem2 = 1869;
                public int giftreward = 50;

                public string bbqregion = "bbq";
                public int bbqcd = 86400;
                public int bbqitem1 = 2267;
                public int bbqitem2 = 353;

                public int startermage = 3069;
                public int starterwarrior = 280;
                public int starterranger = 3492;
                public int startersummoner = 1309;
                public int startercd = 10800;                

                public int mgcd = 120;
                public int mgcost = 1000;
                public int[] mgexlcude = { 17, 18, 19, 20, 22, 35, 37, 38, 54, 68, 85, 105, 106, 107, 108, 113, 114, 113, 123, 124, 125, 126, 127, 128, 129, 130, 131, 134, 135, 136, 139, 142, 143, 144, 145, 158, 159, 160, 162, 166, 172, 178, 207, 208, 209, 212, 213, 214, 215, 216, 227, 228, 229, 245, 246, 247, 248, 251, 253, 262, 263, 264, 265, 269, 276, 281, 282, 288, 290, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 325, 326, 327, 328, 338, 339, 340, 344, 345, 346, 353, 354, 368, 369, 370, 372, 373, 376, 379, 380, 381, 382, 383, 385, 386, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 422, 437, 438, 439, 440, 441, 453, 460, 461, 462, 463, 466, 467, 468, 473, 474, 475, 476, 477, 491, 492, 493, 507, 517, 521, 2889, 2890, 2891, 2892, 2893, 2894, 2895, 3564 };

                #region Buffs
                public int buff1cd = 60;
                public int buff2cd = 60;
                public int buff3cd = 60;
                public int buff4cd = 60;
                public int buff1cost = 150;
                public int buff2cost = 200;
                public int buff3cost = 350;
                public int buff4cost = 500;
                #endregion

                #region Trials
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

                public string lab1region = "lab1";
                public int lab1reward = 68;
                public string lab2region = "lab2";
                public int lab1npc1 = 13;
                public int lab1npc2 = 95;
                public int lab2npc1 = 513;
                public int lab2npc2 = 509;
                public int trial30npc = 51;
                /*
                public string trial60shrineregion = "trial60pre";
                public string trial60mageshrinegroup = "mage59";
                public string trial60warriorshrinegroup = "warrior59";
                public string trial60rangershrinegroup = "ranger59";
                public string trial60summonershrinegroup = "summoner59";
                public string trial60terrarianshrinegroup = "terrarian59"; 
                */
                public string trial60mageregion = "trial60mage";
                public string trial60warriorregion = "trial60warrior";
                public string trial60rangerregion = "trial60ranger";
                public string trial60summonerregion = "trial60summoner";
                public string trial60terrarianregion = "trial60ranger";
                public string trial60magegroup = "mage59";
                public string trial60rangergroup = "ranger59";
                public string trial60warriorgroup = "warrior59";
                public string trial60summonergroup = "summoner59";
                public string trial60terrariangroup = "terrarian59";
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
            #endregion

                #region Restricted mounts
                /*
                public int[] RestrictedMountIds = { 90, 128, 129, 130, 131, 132, 141, 143, 162, 168};
                */
                #endregion
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
