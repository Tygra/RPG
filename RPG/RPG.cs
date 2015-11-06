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
using TShockAPI.DB;
using Wolfje.Plugins.SEconomy;
using Wolfje.Plugins.SEconomy.Journal;
using Newtonsoft.Json;

namespace RPG
{
    [ApiVersion(1, 22)]
    //To-do
    //Test shitty trial loops
    //Trial sign update
    //Adventure cooldown to first instead of three
    //Vip buff/info
    public class RPG : TerrariaPlugin
    {
        #region Info & other things
        public DateTime LastCheck = DateTime.UtcNow;
        public DateTime SLastCheck = DateTime.UtcNow;
        public GPlayer[] Playerlist = new GPlayer[256];
        DateTime DLastCheck = DateTime.UtcNow;
        public TShockAPI.DB.Region Region { get; set; }
        public override string Name
        { get { return "RPG Commands"; } }
        public override string Author
        { get { return "Tygra"; } }
        public override string Description
        { get { return "Geldar RPG Commads"; } }
        public override Version Version
        { get { return new Version(1, 0); } }

        public RPG(Main game)
            : base(game)
        {
            Order = 1;
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("geldar.admin", Reloadcfg, "rpgreload"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Adv, "adventure"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Teleport, "teleport"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Story, "story"));
            Commands.ChatCommands.Add(new Command(Tutorial, "tutorial"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Quests, "quest"));
            Commands.ChatCommands.Add(new Command("geldar.trial", Trial, "trial"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Exban, "exban"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Exui, "exui"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Clearall, "ca"));
            Commands.ChatCommands.Add(new Command("geldar.admin", ResetWorldStats, "resetworldstats"));
            Commands.ChatCommands.Add(new Command(staff, "staff"));
            Commands.ChatCommands.Add(new Command("geldar.level30", Facepalm, "facepalm"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Slapall, "slapall"));
            Commands.ChatCommands.Add(new Command("geldar.vip", VIP, "vip"));
            Commands.ChatCommands.Add(new Command(Geldar, "geldar"));
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            ServerApi.Hooks.GameUpdate.Register(this, Cooldowns);
            if (!Config.ReadConfig())
            {
                TShock.Log.ConsoleError("Delete config because it failed to load.");
            }
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameUpdate.Deregister(this, Cooldowns);
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnLeave);
            }

        }
        #endregion

        #region Playerlist Join/Leave
        public void OnJoin(JoinEventArgs args)
        {
            Playerlist[args.Who] = new GPlayer(args.Who);
        }

        public void OnLeave(LeaveEventArgs args)
        {
            Playerlist[args.Who] = null;
        }
        #endregion

        #region Cooldown
        private void Cooldowns(EventArgs args)
        {
            if ((DateTime.UtcNow - LastCheck).TotalSeconds >= 1)
            {
                LastCheck = DateTime.UtcNow;
                foreach ( var player in Playerlist)
                {
                    if (player == null)
                    {
                        continue;
                    }
                    if (player.pyramid1cd > 0)
                    {
                        player.pyramid1cd--;
                    }
                    if (player.pyramid2cd > 0)
                    {
                        player.pyramid2cd--;
                    }
                    if (player.pyramid3cd > 0)
                    {
                        player.pyramid3cd--;
                    }
                    if (player.pyramid4cd > 0)
                    {
                        player.pyramid4cd--;
                    }
                    if (player.pyramid5cd > 0)
                    {
                        player.pyramid5cd--;
                    }
                    if (player.pyramid6cd > 0)
                    {
                        player.pyramid6cd--;
                    }
                    if (player.pyramid7cd > 0)
                    {
                        player.pyramid7cd--;
                    }
                    if (player.pyramid8cd > 0)
                    {
                        player.pyramid8cd--;
                    }
                    if (player.ice1cd > 0)
                    {
                        player.ice1cd--;
                    }
                    if (player.ice2cd > 0)
                    {
                        player.ice2cd--;
                    }
                    if (player.ice3cd > 0)
                    {
                        player.ice3cd--;
                    }
                    if (player.ice4cd > 0)
                    {
                        player.ice4cd--;
                    }
                    if (player.ice5cd > 0)
                    {
                        player.ice5cd--;
                    }
                    if (player.ice6cd > 0)
                    {
                        player.ice6cd--;
                    }
                    if (player.corr1cd > 0)
                    {
                        player.corr1cd--;
                    }
                    if (player.corr2cd > 0)
                    {
                        player.corr2cd--;
                    }
                    if (player.corr3cd > 0)
                    {
                        player.corr3cd--;
                    }
                    if (player.corr4cd > 0)
                    {
                        player.corr4cd--;
                    }
                    if (player.crim1cd > 0)
                    {
                        player.crim1cd--;
                    }
                    if (player.crim2cd > 0)
                    {
                        player.crim2cd--;
                    }
                    if (player.crim3cd > 0)
                    {
                        player.crim3cd--;
                    }
                    if (player.crim4cd > 0)
                    {
                        player.crim4cd--;
                    }
                    if (player.jadv1cd > 0)
                    {
                        player.jadv1cd--;
                    }
                    if (player.jadv2cd > 0)
                    {
                        player.jadv2cd--;
                    }
                    if (player.jadv3cd > 0)
                    {
                        player.jadv3cd--;
                    }
                    if (player.jadv4cd > 0)
                    {
                        player.jadv4cd--;
                    }
                    if (player.jadv5cd > 0)
                    {
                        player.jadv5cd--;
                    }
                    if (player.space1cd > 0)
                    {
                        player.space1cd--;
                    }
                    if (player.space2cd > 0)
                    {
                        player.space2cd--;
                    }
                    if (player.space3cd > 0)
                    {
                        player.space3cd--;
                    }
                    if (player.space4cd > 0)
                    {
                        player.space4cd--;
                    }
                    if (player.hallow1cd > 0)
                    {
                        player.hallow1cd--;
                    }
                    if (player.hallow2cd > 0)
                    {
                        player.hallow2cd--;
                    }
                    if (player.hallow3cd > 0)
                    {
                        player.hallow3cd--;
                    }
                    if (player.hallow4cd > 0)
                    {
                        player.hallow4cd--;
                    }
                    if (player.hallow5cd > 0)
                    {
                        player.hallow5cd--;
                    }
                    if (player.girocd > 0)
                    {
                        player.girocd--;
                    }
                    if (player.greekonecd > 0)
                    {
                        player.greekonecd--;
                    }
                    if (player.cavecd > 0)
                    {
                        player.cavecd--;
                    }
                    if (player.lab1cd > 0)
                    {
                        player.lab1cd--;
                    }
                    if (player.lab2cd > 0)
                    {
                        player.lab2cd--;
                    }
                    if (player.hiddencd > 0)
                    {
                        player.hiddencd--;
                    }
                    if (player.facepalmcd > 0)
                    {
                        player.facepalmcd--;
                    }
                    if (player.slapallcd > 0)
                    {
                        player.slapallcd--;
                    }
                }
            }
        }
        #endregion

        #region Staffcommands

        #region Extended ban
        private void Exban(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax: /exban \"<player name>\"");
            }
            else
            {
                string text = args.Parameters[0];
                Ban banByName = TShock.Bans.GetBanByName(text, true);
                if (banByName == null)
                {
                    args.Player.SendErrorMessage("No bans by this name were found.");
                }
                else
                {
                    args.Player.SendInfoMessage(string.Concat(new string[] { "Account name: ", banByName.Name, " (", banByName.IP, ")" }));
                    args.Player.SendInfoMessage("Date banned: " + banByName.Date);
                    if (banByName.Expiration != "")
                    {
                        args.Player.SendInfoMessage("Expiration date: " + banByName.Expiration);
                    }
                    args.Player.SendInfoMessage("Banned by: " + banByName.BanningUser);
                    args.Player.SendInfoMessage("Reason: " + banByName.Reason);
                }
            }
        }
        #endregion

        #region Exui
        private void Exui(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {
                string text = string.Join(" ", args.Parameters);
                if (text != null & text != "")
                {
                    User userByName = TShock.Users.GetUserByName(text);
                    if (userByName != null)
                    {
                        args.Player.SendMessage("Query result: ", Color.Goldenrod);
                        args.Player.SendMessage(string.Format("User {0} exists.", text), Color.SkyBlue);
                        try
                        {
                            DateTime dateTime = DateTime.Parse(userByName.Registered);
                            DateTime dateTime2 = DateTime.Parse(userByName.LastAccessed);
                            List<string> list = JsonConvert.DeserializeObject<List<string>>(userByName.KnownIps);
                            string arg = list[list.Count - 1];
                            args.Player.SendMessage(string.Format("{0}'s group is {1}.", text, userByName.Group), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0}'s last known IP is {1}.", text, arg), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0} registered on {1}.", text, dateTime.ToShortDateString()), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0} was last seen {1}.", text, dateTime2.ToShortDateString(), dateTime2.ToShortTimeString()), Color.SkyBlue);
                        }
                        catch
                        {
                            DateTime dateTime = DateTime.Parse(userByName.Registered);
                            args.Player.SendMessage(string.Format("{0}'s group is {1}.", text, userByName.Group), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0} registered on {1}.", text, dateTime.ToShortDateString()), Color.SkyBlue);
                        }
                    }
                    else
                    { args.Player.SendMessage(string.Format("User {0} does not exist.", text), Color.DeepPink); }
                }
                else
                { args.Player.SendErrorMessage("Syntax: /exui \"<player name>\"."); }
            }
            else
            { args.Player.SendErrorMessage("Syntax: /exui \"<player name>\"."); }
        }
        #endregion

        #region Clearall
        private void Clearall(CommandArgs args)
        {
            TShockAPI.Commands.HandleCommand(args.Player, "/clear item 9000");
            TShockAPI.Commands.HandleCommand(args.Player, "/clear projectile 9000");
        }
        #endregion

        #region ResetWorldStats

        public static void ResetWorldStats(CommandArgs args)
        {
            NPC.downedBoss1 = false;
            NPC.downedBoss2 = false;
            NPC.downedBoss3 = false;
            NPC.downedQueenBee = false;
            NPC.downedSlimeKing = false;
            NPC.downedMechBossAny = false;
            NPC.downedMechBoss1 = false;
            NPC.downedMechBoss2 = false;
            NPC.downedMechBoss3 = false;
            NPC.downedFishron = false;
            NPC.downedMartians = false;
            NPC.downedAncientCultist = false;
            NPC.downedMoonlord = false;
            NPC.downedHalloweenKing = false;
            NPC.downedHalloweenTree = false;
            NPC.downedChristmasIceQueen = false;
            NPC.downedChristmasSantank = false;
            NPC.downedChristmasTree = false;
            NPC.downedPlantBoss = false;
            NPC.downedGoblins = false;
            NPC.downedClown = false;
            NPC.downedFrost = false;
            NPC.downedPirates = false;
            NPC.savedAngler = false;
            NPC.downedGolemBoss = false;
            WorldGen.shadowOrbSmashed = false;
            WorldGen.altarCount = 0;
            WorldGen.shadowOrbCount = 0;
            args.Player.SendSuccessMessage("The World Generation stats have been reset.");
        }

        #endregion

        #endregion

        #region Other commands

        #region staff
        public void staff(CommandArgs args)
        {
            List<TSPlayer> list = new List<TSPlayer>(TShock.Players).FindAll((TSPlayer t) => t != null && t.Group.HasPermission("geldar.mod"));
            if (list.Count == 0)
            {
                args.Player.SendErrorMessage("No staff members currently online. If you have a problem check our website at www.geldar.net.");
            }
            else
            {
                args.Player.SendMessage("[Currently online staff members]", Color.Goldenrod);
                foreach (TSPlayer current in list)
                {
                    if (current != null)
                    {
                        Color color = new Color((int)current.Group.R, (int)current.Group.G, (int)current.Group.B);
                        args.Player.SendMessage(string.Format("{0}{1}", current.Group.Prefix, current.Name), color);
                    }
                }
            }
        }
        #endregion

        #region Facepalm
        private void Facepalm(CommandArgs args)
        {
            var player = Playerlist[args.Player.Index];
            if (player.facepalmcd != 0)
            {
                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.facepalmcd));
                args.Player.SendErrorMessage("Chill, facepalming repeatedly can be fatal.");
                return;
            }
            else
            {
                TSPlayer.All.SendMessage(string.Format("{0} facepalmed.", args.Player.Name), Color.Goldenrod);
                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                {
                    player.facepalmcd = Config.contents.facepalmcd;
                }
            }
        }
        #endregion

        #region Slapall
        private void Slapall(CommandArgs args)
        {
            var player = Playerlist[args.Player.Index];
            if (player.slapallcd != 0)
            {
                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.slapallcd));
                args.Player.SendErrorMessage("The slapdemic has to wait");
                return;
            }
            else
            {                
                TSPlayer.All.DamagePlayer(1);
                TSPlayer.All.SendMessage(string.Format("{0} slapped everyone.", args.Player.Name), Color.Goldenrod);
                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                {
                    player.slapallcd = Config.contents.slapallcd;
                }
            }
        }
        #endregion

        #endregion

        #region Tutorial
        private void Tutorial(CommandArgs args)
        {
            if(args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: If you wish to be teleported to the tutorial zone, use /teleport tutorial", Color.Goldenrod);
                args.Player.SendMessage("Info: For some housing information use /tutorial housing.", Color.Goldenrod);
                args.Player.SendMessage("Info: For a trading tutorial use /tutorial trading.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region housing
                case "housing":
                    {
                        args.Player.SendMessage("------------------------ Housing Tutorial ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("You areallowed to have a house from level 20, either in the above or the underground zone.", Color.SkyBlue);
                        args.Player.SendMessage("The maximum size allowed is 15 blocks wide,12 blocks high, walls included.", Color.SkyBlue);
                        args.Player.SendMessage("Follow the guidelines at /house set 1 and /house set 2.", Color.SkyBlue);
                        args.Player.SendMessage("After you marked the spots use /house add housename. Change housename to your desired house name.", Color.SkyBlue);
                        args.Player.SendMessage("Don't forget to check the housing rules either on our website or with /geldar.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region trading
                case "trading":
                    {
                        args.Player.SendMessage("------------------------ Trading Tutorial ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("From level 5 you can sell and buy item on trade.", Color.SkyBlue);
                        args.Player.SendMessage("Example: /trade add \"cactus sword\" 1 100.", Color.SkyBlue);
                        args.Player.SendMessage("This case you are selling 1 Cactus sword for 100 Terra Coins.", Color.SkyBlue);
                        args.Player.SendMessage("If the item name is two or more word you will need quotation marks around the item name.", Color.SkyBlue);
                        args.Player.SendMessage("/trade check - to check your listed trades; /trade list - to check the full trade list", Color.SkyBlue);
                        args.Player.SendMessage("/trade collect - to collect money from finished trades; /trade cancel ID - to cancel a trade.", Color.SkyBlue);
                        args.Player.SendMessage("/trade accept ID - to buy something. You can get the ID from trade list or /trade search.", Color.SkyBlue);
                        args.Player.SendMessage("You will need qoutation marks when using /trade search \"item name\" too.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter and use the up arrow to scroll the chat upwards.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region tutclass
                case "class":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutclassregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Class tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Class tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6221 983");
                        }
                    }
                    break;
                #endregion

                #region tutgear
                case "gear":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutgearregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Gear tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Gear tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6213 992");
                        }
                    }
                    break;
                #endregion

                #region tutmine
                case "mine":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutmineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Mining tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Mining tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6225 1007");
                        }
                    }
                    break;
                #endregion

                #region tuttrade
                case "trade":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tuttraderegion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Trading tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Trading tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6250 1009");
                        }
                    }
                    break;
                #endregion

                #region tuthouse
                case "house":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tuthouseregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Housing tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Housing tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6216 1015");
                        }
                    }
                    break;
                    #endregion

                #region dropretry
                case "dropretry":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutmineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6216 1015");
                        }
                    }
                    break;
                #endregion

                #region tutdrop
                case "drop":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutmineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the correct shaft of the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the correct shaft of the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6230 1030");
                        }
                    }
                    break;
                #endregion
            }
        }
        #endregion

        #region Trials
        private void Trial(CommandArgs args)
        {            
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: At level 29, 59, 69 and 79 you have to complete a trial. Below you can find the available commands.", Color.Goldenrod);
                args.Player.SendMessage("Info: If you wish to skip the trials use /trial skip to get more information about it.", Color.Goldenrod);
                args.Player.SendMessage("Info: The commands to finish the trial can be found on the last sign of the trial.", Color.Goldenrod);

                return;
            }

            switch (args.Parameters[0])
            {
                #region Trial 30
                case "trial30":
                    {
                        #region Trial 30 region check
                        Region region = TShock.Regions.GetRegionByName(Config.contents.trial30region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check story signs to get hints.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check story signs to get hints.");
                            return;
                        }
                        #endregion

                        #region Trial 30 group check
                        if (args.Player.Group.Name == Config.contents.lab2magegroup || args.Player.Group.Name == Config.contents.lab2rangergroup || args.Player.Group.Name == Config.contents.lab2warriorgroup || args.Player.Group.Name == Config.contents.lab2summonergroup)
                        {
                            #endregion

                            #region Trial 30 mage
                            if (args.Player.Group.Name == Config.contents.lab2magegroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Sorcerer", Color.SkyBlue);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " mage30");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /firework " + args.Player.Name);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1071 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 4");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 2349 9");
                            }
                            #endregion

                            #region Trial 30 ranger
                            if (args.Player.Group.Name == Config.contents.lab2rangergroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Marksman", Color.SkyBlue);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " ranger30");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /firework " + args.Player.Name);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1071 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 4");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 2349 9");
                            }
                            #endregion

                            #region Trial 30 warrior
                            if (args.Player.Group.Name == Config.contents.lab2warriorgroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Knight", Color.SkyBlue);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " warrior30");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /firework " + args.Player.Name);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1071 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 4");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 2349 9");
                            }
                            #endregion

                            #region Trial 30 summoner
                            if (args.Player.Group.Name == Config.contents.lab2summonergroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Beckoner", Color.SkyBlue);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " summoner30");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /firework " + args.Player.Name);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1071 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 4");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 2349 9");
                            }
                            #endregion
                        }

                        else
                        {
                            args.Player.SendErrorMessage("You need to complete the second part of the trial before this.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Trial 60
                case "trial60":
                    {
                        #region Trial 60 region check
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup)
                        {
                            #endregion

                            #region Trial 60 mage
                            if (args.Player.Group.Name == Config.contents.trial60magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60mageregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60magegroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " mage60");
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Archon", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }                               
                            }
                            #endregion

                            #region Trial 60 ranger
                            if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60rangerregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Ranger room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Ranger room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60rangergroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " ranger60");
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Deadshot", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion

                            #region Trial 60 warrior
                            if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60warriorregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Warrior room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Warrior room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60warriorgroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " warrior60");
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Blademaster", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion

                            #region Trial 60 summoner
                            if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60summonerregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60summonergroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " summoner60");
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Archon", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59.");
                            return;
                        }                        
                    }
                    break;

                #endregion

                #region Trial skip

                #region Level 30 trial skip
                case "skip30":
                    {
                        if (args.Player.Group.Name == Config.contents.trial30magegroup || args.Player.Group.Name == Config.contents.trial30rangergroup || args.Player.Group.Name == Config.contents.trial30warriorgroup || args.Player.Group.Name == Config.contents.trial30summonergroup)
                        {
                            #region Trial 30 mage skip
                            if (args.Player.Group.Name == Config.contents.trial30magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " mage30");
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 ranger skip
                            if (args.Player.Group.Name == Config.contents.trial30rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " ranger30");
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 warrior skip
                            if (args.Player.Group.Name == Config.contents.trial30warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " warrior30");
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 summoner skip
                            if (args.Player.Group.Name == Config.contents.trial30summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " summoner30");
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion
                        }

                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 29");
                            return;
                        }

                    }
                    break;
                #endregion

                #region Level 60 trial skip
                case "skip60":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup)
                        {
                            #region Trial 60 mage skip
                            if (args.Player.Group.Name == Config.contents.trial60magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " mage60");
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Archon", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 ranger skip
                            if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " ranger60");
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Deadshot", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 warrior skip
                            if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " warrior60");
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Blademaster", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 summoner skip
                            if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at spawn with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " summoner60");
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Animist", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion
                        }
                    }
                    break;
                    #endregion
                    #endregion
            }
        }
        #endregion

        #region Story

        private void Story(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("If want to start the story use /teleport story.", Color.Goldenrod);
                args.Player.SendMessage("Sometimes you need to read the signs twice. There are hints which will lead you to the next part.", Color.Goldenrod);
                args.Player.SendMessage("For a tutorial on how to do the first part check www.geldar.net Tutorials forum.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region greekone
                case "greekone":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.greekoneregion);
                        if (player.greekonecd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.greekonecd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead.");
                            return;
                        }
                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "sudo -f " + args.Player.Name + " /item 3199 1");
                            args.Player.SendMessage("You just looted an Ice Mirror", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.greekonecd = Config.contents.greekonecd;
                            }
                        }
                    }
                    break;
                #endregion

                #region hidden
                case "hidden":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hiddenregion);
                        if (player.hiddencd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hiddencd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("Youa re not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("Youa re not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1579 1");
                            args.Player.SendMessage("You just looted Flurry Boots!", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.hiddencd = Config.contents.hiddencd;
                            }
                        }
                    }
                    break;
                #endregion

                #region lab1
                case "lab1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.lab1region);
                        if (player.lab1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.lab1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.trial30magegroup || args.Player.Group.Name == Config.contents.trial30rangergroup || args.Player.Group.Name == Config.contents.trial30warriorgroup || args.Player.Group.Name == Config.contents.trial30summonergroup)
                        {
                            if (args.Player.Group.Name == Config.contents.trial30magegroup)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 70 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " mage29_1");
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab1cd = Config.contents.lab1cd;
                                }
                            }
                            if (args.Player.Group.Name == Config.contents.trial30rangergroup)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 70 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " ranger29_1");
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab1cd = Config.contents.lab1cd;
                                }
                            }
                            if (args.Player.Group.Name == Config.contents.trial30warriorgroup)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 70 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " warrior29_1");
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab1cd = Config.contents.lab1cd;
                                }
                            }
                            else if (args.Player.Group.Name == Config.contents.trial30summonergroup)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 70 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " summoner29_1");
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab1cd = Config.contents.lab1cd;
                                }
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 29 to start the trial.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region lab2
                case "lab2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.lab2region);
                        if (player.lab2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.lab2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.lab1magegroup || args.Player.Group.Name == Config.contents.lab1rangergroup || args.Player.Group.Name == Config.contents.lab1warriorgroup || args.Player.Group.Name == Config.contents.lab1summonergroup)
                        {
                            if (args.Player.Group.Name == Config.contents.lab1magegroup)
                            {
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /slap " + args.Player.Name);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " mage29_2");
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab2cd = Config.contents.lab2cd;
                                }
                            }
                            if (args.Player.Group.Name == Config.contents.lab1rangergroup)
                            {
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /slap " + args.Player.Name);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " ranger29_2");
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab2cd = Config.contents.lab2cd;
                                }
                            }
                            if (args.Player.Group.Name == Config.contents.lab1warriorgroup)
                            {
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /slap " + args.Player.Name);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " warrior29_2");
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab2cd = Config.contents.lab2cd;
                                }
                            }
                            if (args.Player.Group.Name == Config.contents.lab1summonergroup)
                            {
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /slap " + args.Player.Name);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /user group " + args.Player.Name + " summoner29_2");
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.lab2cd = Config.contents.lab2cd;
                                }
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to complete the frist part of the trial before this.");
                            return;
                        }
                    }
                    break;
                #endregion
            }
        }

        #endregion

        #region Quests
        private void Quests(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Each quest has it's own subcommand, which can be found at the quest's destination.", Color.Goldenrod);
                args.Player.SendMessage("To finish some of the quests you need certain items to be in your inventory.", Color.Goldenrod);
                args.Player.SendMessage("You can check the available quests on the notice boards next to spawn.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region giro
                case "giro":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.giroregion);
                        if (player.girocd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.girocd));
                            return;
                        }

                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Professor Giro's house.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Professor Giro's house.");
                            return;
                        }

                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.giroreward, BankAccountTransferOptions.AnnounceToReceiver, "Professor Giro Reward", "Giro reward");
                            if(!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.girocd = Config.contents.girocd;
                            }
                        }
                    }
                    break;
                #endregion

                #region mill
                case "mill":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.millregion);
                        if (player.millcd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.millcd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Melody's Farmstead.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Melody's Farmstead.");
                            return;
                        }
                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1238 1");
                            args.Player.SendMessage("You just looted a Sapphire Hook!", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.millcd = Config.contents.millcd;
                            }
                        }
                    }
                    break;

                #endregion

                #region gloomy cave
                case "cave":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.caveregion);
                        if (player.cavecd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.cavecd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 3520 1");
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 19 12");
                            args.Player.SendMessage("You just looted a Gold Broadsword and 12 Gold Bars!", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.cavecd = Config.contents.cavecd;
                            }
                        }

                    }
                    break;

                #endregion
            }

        }

        #endregion

        #region Teleport
        private void Teleport(CommandArgs args)
        {
            if(args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: Use the commands below.", Color.Goldenrod);
                args.Player.SendMessage("Info: /teleport adventure - Teleports you to the adventure tower.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport tutorial - Teleports you to the Tutorial zone.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport story - Teleports you to the very first part of the story.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport oasis - Teleports you to the Poised Oasis.", Color.SkyBlue);
                args.Player.SendMessage("Info: Oasis teleport requirements: 250 Terra coins, Level 30, Warehouse teleport pad.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Adventure teleport
                case "adventure":
                    {
                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 445 875");
                        args.Player.SendMessage("You have been teleported to the Adventure Tower.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region Tutorial teleport
                case "tutorial":
                    {
                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6223 962");
                        args.Player.SendMessage("Yoou have been teleported to the Tutorial zone.", Color.Goldenrod);
                    }
                    break;

                #endregion

                #region Oasis teleport
                case "oasis":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.oasiscost;
                        Money moneyamount2 = Config.contents.oasiscost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to teleport to the oasis. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }

                        Region region = TShock.Regions.GetRegionByName(Config.contents.oasisregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Warehouse Teleportpad");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Warehouse Teleportpad");
                            return;
                        }

                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the oasis teleport.", moneyamount2, args.Player.Name), string.Format("Oasis Teleport"));
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 1345 456");
                            args.Player.SendMessage("You've paid 250 Terra Coins to be teleported to the Poisoned Oasis.", Color.Goldenrod);
                        }
                    }
                    break;

                #endregion

                #region Story teleport
                case "story":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.storyregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Spawn/Landfall");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Spawn/Landfall");
                            return;
                        }

                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /tppos 6096 659");
                        }
                    }   
                    break;

                #endregion
            }
        }

        #endregion

        #region Adventure command
        private void Adv(CommandArgs args)
        {
            if(args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: Each adventure subcommand can be used at the sign, at the correct place.", Color.Goldenrod);
                args.Player.SendMessage("Example: You are in the pyramid first sign. /adventure pyramid1", Color.Goldenrod);
                args.Player.SendMessage("Info: Each command has a unique cooldown of one day(24 hours).", Color.Goldenrod);
                args.Player.SendMessage("Info: Be sure to have at least 4 free inventory slots!", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {

                #region pyramid1
                case "pyramid1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid1cd));
                            return;
                        }                       
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 159 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 790 1");
                                args.Player.SendMessage("You just looted a Golden Key, a Shiny Red Balloon and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid1cd = Config.contents.pyramid1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid2
                case "pyramid2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 791 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 116 40");
                                args.Player.SendMessage("You just looted 2 Golden Keys, 40 Meteories and an Omega Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid2cd = Config.contents.pyramid2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid3
                case "pyramid3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 285 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 789 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 5");
                                args.Player.SendMessage("You just looted a Golden Key, an Aglet an Ankh Banner and 5 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid3cd = Config.contents.pyramid3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid4
                case "pyramid4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 857 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 791 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 790 1");
                                args.Player.SendMessage("You just looted a Golden Key, a Sandstorm in a Bottle, and Omega Banner and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid4cd = Config.contents.pyramid4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid5
                case "pyramid5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid5region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid5cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 848 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 866 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 10");
                                args.Player.SendMessage("You just looted a Golden Key, a Pharaoh Set and 10 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid5cd = Config.contents.pyramid5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid6
                case "pyramid6":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid6region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid6cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid6cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 49 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 791 1");
                                args.Player.SendMessage("You just looted a Golden Key, a Band of Regeneration and an Omega Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid6cd = Config.contents.pyramid6cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid7
                case "pyramid7":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid7region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid7cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid7cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 159 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 790 1");
                                args.Player.SendMessage("You just looted a Golden Key, a Shiny Red Balloon and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid7cd = Config.contents.pyramid7cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid8
                case "pyramid8":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid8region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (player.pyramid8cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid8cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 159 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 790 1");
                                args.Player.SendMessage("You just looted a Golden Key, a Shiny Red Balloon and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid8cd = Config.contents.pyramid8cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice1
                case "ice1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (player.ice1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice1cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1319 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 3");
                                args.Player.SendMessage("You just looted a Golden Key, a Snowball Cannon and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice1cd = Config.contents.ice1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice2
                case "ice2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (player.ice2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 950 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 3");
                                args.Player.SendMessage("You just looted a Golden Key, Ice Skates and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice2cd = Config.contents.ice2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice3
                case "ice3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (player.ice3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 987 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 3");
                                args.Player.SendMessage("You just looted a Golden Key, a Blizzard in a Bottle and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice3cd = Config.contents.ice3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice4
                case "ice4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (player.ice4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 724 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 3");
                                args.Player.SendMessage("You just looted a Golden Key, an Ice Blade and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice4cd = Config.contents.ice4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice5
                case "ice5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice5region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (player.ice5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice5cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 997 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 3");
                                args.Player.SendMessage("You just looted a Golden Key, an Extractinator and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice5cd = Config.contents.ice5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice6
                case "ice6":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice6region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (player.ice6cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice6cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 670 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 188 3");
                                args.Player.SendMessage("You just looted a Golden Key, an Ice Boomerang and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice6cd = Config.contents.ice6cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr1
                case "corr1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (player.corr1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr1cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 522 15");
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 15 Cursed Flames !", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr1cd = Config.contents.corr1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr2
                case "corr2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (player.corr2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 556 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 499 10");
                                args.Player.SendMessage("You just looted 3 Golden Keys, a Mechanical Worm and 10 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr2cd = Config.contents.corr2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr3
                case "corr3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (player.corr3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1225 10");
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 10 Hallowed Bars!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr3cd = Config.contents.corr3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr4
                case "corr4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (player.corr4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1819 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1820 1");
                                args.Player.SendMessage("You just looted 3 Golden Keys, and a Reaper Costume!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr4cd = Config.contents.corr4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim1
                case "crim1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }

                        if (player.crim1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim1cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 556 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 499 5");
                                args.Player.SendMessage("You just looted 3 Golden Keys, a Mechanical Worm and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim1cd = Config.contents.crim1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim2
                case "crim2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }

                        if (player.crim2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1225 10");
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 10 Hallowed Bars!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim2cd = Config.contents.crim2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim3
                case "crim3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }

                        if (player.crim3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1332 15");
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 15 Ichor!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim3cd = Config.contents.crim3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim4
                case "crim4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }

                        if (player.crim4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 3");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1838 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1839 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1840 1");
                                args.Player.SendMessage("You just looted 3 Golden Keys, and a Space Creature Costume!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim4cd = Config.contents.crim4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv1
                case "jadv1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (player.jadv1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv1cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 3360 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 3361 1");
                                args.Player.SendMessage("You just looted 2 Golden Keys a Rich Mahogany Leaf Wand and Living Mahogany Wand!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv1cd = Config.contents.jadv1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv2
                case "jadv2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (player.jadv2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 753 1");
                                args.Player.SendMessage("You just looted 2 Golden Keys and Seaweed!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv2cd = Config.contents.jadv2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv3
                case "jadv3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (player.jadv3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 3068 1");
                                args.Player.SendMessage("You just looted 2 Golden Keys and a Guide to Plant Fiber Cordage!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv3cd = Config.contents.jadv3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv4
                case "jadv":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (player.jadv4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 964 1");
                                args.Player.SendMessage("You just looted 2 Golden Keys and a Boomstick!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv4cd = Config.contents.jadv4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv5
                case "jadv5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv5region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (player.jadv5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv5cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 2204 1");
                                args.Player.SendMessage("You just looted 2 Golden Keys and a Honey Dispenser!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv5cd = Config.contents.jadv5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Space1
                case "space1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (player.space1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space1cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 ");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 117 10");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 179 15");
                                args.Player.SendMessage("You just looted a Golden Key, 10 Meteorite Bars, and 5 Emeralds!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space1cd = Config.contents.space1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Space2
                case "space2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (player.space2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 75 10");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 181 5");
                                args.Player.SendMessage("You just looted a Golden Key, 10 Fallen Stars, and 5 Amethysts!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space2cd = Config.contents.space2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Space3
                case "space3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (player.space3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 75 10");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 177 5");
                                args.Player.SendMessage("You just looted a Golden Key, 10 Fallen Stars, and 5 Sapphires!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space3cd = Config.contents.space3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Space4
                case "space4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (player.space4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 117 15");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 178 5");
                                args.Player.SendMessage("You just looted a Golden Key, 15 Meteorite Bars, and 5 Rubies!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space4cd = Config.contents.space4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow1
                case "hallow1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (player.hallow1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow1cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1725 15");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 499 5");
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow1cd = Config.contents.hallow1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow2
                case "hallow2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (player.hallow2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow2cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1ó2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1725 15");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 3548 15");
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 15 Happy Grenades!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow2cd = Config.contents.hallow2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow3
                case "hallow3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow3region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (player.hallow3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow3cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1774 1");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 499 5");
                                args.Player.SendMessage("You just looted 2 Golden Keys, 5 Greater Healing Potions, and a Goodie Bag!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow3cd = Config.contents.hallow3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow4
                case "hallow4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow4region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (player.hallow4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow4cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1725 15");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 499 5");
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow4cd = Config.contents.hallow4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow5
                case "hallow5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow5region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (player.hallow5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow5cd));
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 2");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 1725 15");
                                TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 499 5");
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow5cd = Config.contents.hallow5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                
                
            }
        }
        #endregion   

        #region VIP
        private void VIP(CommandArgs args)
        {

        }
        #endregion

        #region Geldar
        private void Geldar(CommandArgs args)
        {
            //Don't judge, coun't do the 2 parameter command tree
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: For the most basic commands use /geldar info.", Color.Goldenrod);
                args.Player.SendMessage("info: For rules use /geldar <general/chat/housing/itemdrop/further>", Color.Goldenrod);
                args.Player.SendMessage("Info: Be warned! The full list of rules can be found on our website www.geldar.net", Color.Goldenrod);
                args.Player.SendMessage("Info: The rules here are just the most important ones, shortened to fit.", Color.Goldenrod);
                return;
            }
            
            switch (args.Parameters[0])
            {
                case "info":
                    {
                        args.Player.SendMessage("Welcome to our server, Geldar.", Color.Goldenrod);
                        args.Player.SendMessage("Info: You need level 10 for mining and level 20 to have a house.", Color.SkyBlue);
                        args.Player.SendMessage("Houses can be built above or under spawn.", Color.SkyBlue);
                        args.Player.SendMessage("Info: You can use /spawn to teleport to the map's spawnpoint.", Color.SkyBlue);
                        args.Player.SendMessage("Info: The server uses an ingame serverside currency name Terra coins.", Color.SkyBlue);
                        args.Player.SendMessage("Info: You need these Terra Coins (tc) to level up, trade, or use ceratin commands.", Color.SkyBlue);
                        args.Player.SendMessage("Info: To check your tc balance use /bank bal. Earn tc by killing monsters.", Color.SkyBlue);
                        args.Player.SendMessage("Info: For tutorials please use /tutorial.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;

                case "general":
                    {
                        args.Player.SendMessage("------------------------ General Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: On the Main map you are not allowed to build arenas on the surface.", Color.SkyBlue);
                        args.Player.SendMessage("Info: No massive terraforming. ( Destroying mountains, making skybridges, walls.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Do not obstruct players in free movement. (walls,barricades,holes)", Color.SkyBlue);
                        args.Player.SendMessage("Info: Afk farms are not allowed. (boxes around you, mob trapholes)", Color.SkyBlue);
                        args.Player.SendMessage("Info: Going afk while you are protected(gaining tc while afk), is not allowed.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Check the \"Is it Cheating\" thread on our forum.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Using any kind of bug/exploit/glitch will get you banned.", Color.SkyBlue);
                        args.Player.SendMessage("Info: We will ban for the smallest grief.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Using modified/hacked clients will get you banned permanently.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;

                case "chat":
                    {
                        args.Player.SendMessage("------------------------ Chat Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: Write in English or you will get muted, kicked or banned.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Don't use offensive, allcaps spammy character names.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Spamming/flooding the chat will get you muted or banned.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Keep the swearing to a minimum.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Racist and discriminative comments will be harshly dealt with.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Advertising anything will get you banned.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;

                case "housing":
                    {
                        args.Player.SendMessage("------------------------ Housing Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: You can only have one house. All your characters must use the same one.", Color.SkyBlue);
                        args.Player.SendMessage("Info: House size limit is 15 blocks wide and 12 blocks high. Walls counted in.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Unprotected houses will be removed after 2 days.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Do not put spikes or anything else on your house that can obstruct players.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Only build houses abouve or under spawn where we marked spots.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Bigger clouds are for more than one player. Build on the side of the island.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Do not overlap houses, do not create one big house with your friends.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Every house is limited to 5 chests. (Piggy banks, safes included)", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;

                case "itemdrop":
                    {
                        args.Player.SendMessage("------------------------ Item Drop Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: You are only allowed to give away vanity, furniture, consumables, money and ammo.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Every item has a tooltip. Check it before dropping.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Treasure bags are not allowed to be given away.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Use /trade to exchange items with others.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;
            }
        }
        #endregion

        #region Config reload

        private void Reloadcfg(CommandArgs args)
        {
            if (Config.ReadConfig())
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
