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
    //Clevel
    //Ilevel
    //Trials
    //Test shitty trial loops
    //Trial sign update
    //Adventure cooldown to first instead of three
    public class RPG : TerrariaPlugin
    {
        #region Info & other things
        public DateTime LastCheck = DateTime.UtcNow;
        public DateTime SLastCheck = DateTime.UtcNow;
        public GPlayer[] Playerlist = new GPlayer[256];
        DateTime DLastCheck = DateTime.UtcNow;
        public TShockAPI.DB.Region Region { get; set; }
        public override string Name
        {
            get
            {
                return "RPG Commands";
            }
        }
        public override string Author
        {
            get
            {
                return "Tygra";
            }
        }
        public override string Description
        {
            get
            {
                return "Geldar RPG Commands";
            }
        }
        public override Version Version
        {
            get
            {
                return new Version("1.0");
            }
        }
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
            Commands.ChatCommands.Add(new Command(Ilevel, "ilevel", "il"));
            Commands.ChatCommands.Add(new Command(Clevel, "Clevel", "cl"));
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
                }
            }
        }
        #endregion

        #region Tutorial
        private void Tutorial(CommandArgs args)
        {
            if(args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: If you wish to be teleported to the tutorial zone, use /teleport tutorial", Color.Goldenrod);
                args.Player.SendMessage("Info: For some housing information use /tutorial house.", Color.Goldenrod);
                args.Player.SendMessage("Info: For a trading tutorial use /tutorial trading.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region house
                case "house":
                    {
                        args.Player.SendMessage("--------------------------------------- Housing Tutorial ---------------------------------------", Color.Goldenrod);
                        args.Player.SendMessage("You areallowed to have a house from level 20, either in the above or the underground zone.", Color.SkyBlue);
                        args.Player.SendMessage("The maximum size allowed is 15 blocks wide,12 blocks high, walls included.", Color.SkyBlue);
                        args.Player.SendMessage("Follow the guidelines at /house set 1 and /house set 2.", Color.SkyBlue);
                        args.Player.SendMessage("After you marked the spots use /house add housename. Change housename to your desired house name.", Color.SkyBlue);
                        args.Player.SendMessage("Don't forget to check the housing rules either on our website or with /rules.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region trading
                case "trading":
                    {
                        args.Player.SendMessage("--------------------------------------- Trading Tutorial ---------------------------------------", Color.Goldenrod);
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
                case "tutclass":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutclassregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Class tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Class tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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
                case "tutgear":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutgearregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Gear tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Gear tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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
                case "tutmine":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutmineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Mining tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Mining tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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
                case "tuttrade":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tuttraderegion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Trading tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Trading tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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
                case "tuthouse":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tuthouseregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Housing tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Housing tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Itemdrop tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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
                case "tutdrop":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutmineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the correct shaft of the Itemdrop tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the correct shaft of the Itemdrop tutorials.");
                            args.Player.SendMessage("Stand on the sign when you execute the command.", Color.Goldenrod);
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

                        #region Trial 30 mage
                        if (args.Player.Group.Name == Config.contents.trial30magegroup)
                        {

                        }
                        #endregion

                        #region Trial 30 warrior
                        if (args.Player.Group.Name == Config.contents.trial30warriorgroup)
                        {

                        }
                        #endregion

                        #region Trial 30 ranger
                        if (args.Player.Group.Name == Config.contents.trial30rangergroup)
                        {

                        }
                        #endregion

                        #region Trial 30 summoner
                        else
                        {

                        }
                        #endregion
                    }
                    break;
                #endregion

                #region Trial skip

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
                        else
                        {
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 70 1");
                            args.Player.SendMessage("You just loote Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.lab1cd = Config.contents.lab1cd;
                            }
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
                        else
                        {
                            args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                            TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /slap " + args.Player.Name);
                            TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.lab2cd = Config.contents.lab2cd;
                            }
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

        #region Clevel
        private void Clevel(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {

            }
        }

        #endregion

        #region Ilevel
        private void Ilevel(CommandArgs args)
        {

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
