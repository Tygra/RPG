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
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            ServerApi.Hooks.GameUpdate.Register(this, Cooldowns);
           // Commands.ChatCommands.Add(new Command(ilevel, "ilevel", "il"));
           // Commands.ChatCommands.Add(new Command(clevel, "clevel", "cl"));
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
                }
            }
        }
        #endregion

        #region Story

        private void Story(CommandArgs args)
        {

        }

        #endregion

        #region Teleport
        private void Teleport(CommandArgs args)
        {
            if(args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: Use the commands below.", Color.Goldenrod);
                args.Player.SendMessage("Info: /teleport adventure - Teleports you to the adventure tower.", Color.SkyBlue);
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

                #region Oasis teleport
                case "oasis":
                    {

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
